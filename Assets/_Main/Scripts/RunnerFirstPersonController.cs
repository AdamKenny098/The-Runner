// RunnerFirstPersonController.cs
// Minimal, independent FPC for "The Runner". Fixed mouse sensitivity.
// Works with either the new Input System or legacy Input.

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RunnerFirstPersonController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Transform the camera rotates around (usually a child). If null, uses Camera.main.transform.")]
    public Transform cameraRoot;

    [Header("Movement")]
    [Tooltip("Meters/second at normal speed.")]
    public float walkSpeed = 4.5f;
    [Tooltip("Meters/second while sprinting.")]
    public float sprintSpeed = 7.5f;
    [Tooltip("How quickly we reach target speed.")]
    public float acceleration = 12f;
    [Tooltip("Horizontal damping when no input.")]
    public float deceleration = 14f;

    [Header("Jump / Gravity")]
    public float jumpHeight = 1.1f;
    public float gravity = -18f;
    [Tooltip("Extra stick-to-ground force for smoother steps.")]
    public float groundedGravity = -2f;
    [Tooltip("Grounded check radius.")]
    public float groundCheckRadius = 0.25f;
    [Tooltip("Offset from feet for ground check.")]
    public float groundCheckOffset = 0.1f;
    [Tooltip("Layers considered ground.")]
    public LayerMask groundLayers = ~0;

    [Header("Look")]
    [Tooltip("Mouse sensitivity (try 0.12–0.25).")]
    public float lookSensitivity = 0.18f;
    [Tooltip("Clamp vertical look (pitch).")]
    public float minPitch = -80f;
    public float maxPitch = 80f;

    [Header("Sprint")]
    [Tooltip("Hold to sprint. If false, LeftShift toggles.")]
    public bool sprintHold = true;

    [Header("Quality of Life")]
    public bool lockCursor = true;

    // Runtime
    private CharacterController _cc;
    private Vector3 _velocity;        // includes vertical
    private Vector2 _inputMove;       // -1..1
    private Vector2 _inputLook;       // raw mouse delta (pixels), or stick delta (normalized)
    private bool _wantsJump;
    private bool _isSprinting;
    private float _pitch;
    private float _currentSpeed;      // horizontal magnitude target

    // Public read-only state
    public bool IsGrounded { get; private set; }
    public bool IsSprinting => _isSprinting;
    public float CurrentHorizontalSpeed => new Vector2(_velocity.x, _velocity.z).magnitude;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        if (!cameraRoot)
        {
            var cam = Camera.main;
            if (cam) cameraRoot = cam.transform;
        }
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        ReadInput();
        ApplyLook();

        IsGrounded = CheckGrounded();

        // Horizontal movement
        Vector3 wishDir = new Vector3(_inputMove.x, 0f, _inputMove.y);
        wishDir = transform.TransformDirection(wishDir).normalized;

        float targetSpeed = (_isSprinting ? sprintSpeed : walkSpeed) * wishDir.magnitude;

        float speedChange = (targetSpeed > _currentSpeed ? acceleration : deceleration) * Time.deltaTime;
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, speedChange);

        Vector3 horizontalVel = wishDir * _currentSpeed;

        // Vertical (gravity/jump)
        if (IsGrounded)
        {
            if (_velocity.y < 0f) _velocity.y = groundedGravity;
            if (_wantsJump)
            {
                _wantsJump = false;
                _velocity.y = Mathf.Sqrt(-2f * gravity * Mathf.Max(0.01f, jumpHeight));
            }
        }
        else
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        _velocity.x = horizontalVel.x;
        _velocity.z = horizontalVel.z;

        _cc.Move(_velocity * Time.deltaTime);
    }

    private void ApplyLook()
    {
        if (!cameraRoot) return;

        // Body yaw (horizontal)
        transform.Rotate(0f, _inputLook.x * lookSensitivity, 0f, Space.Self);

        // Camera pitch (vertical)
        _pitch -= _inputLook.y * lookSensitivity;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        cameraRoot.localEulerAngles = new Vector3(_pitch, 0f, 0f);
    }

    private bool CheckGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * (_cc.height * 0.5f - _cc.radius + groundCheckOffset);
        return Physics.CheckSphere(origin, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    // -------- INPUT --------
    private void ReadInput()
    {
#if ENABLE_INPUT_SYSTEM
        // New Input System (polling minimal keys for simplicity)
        var kb = UnityEngine.InputSystem.Keyboard.current;
        var mouse = UnityEngine.InputSystem.Mouse.current;
        var gamepad = UnityEngine.InputSystem.Gamepad.current;

        // Move (WASD) — add simple gamepad fallback (left stick)
        float mx = 0f, mz = 0f;
        if (kb != null)
        {
            if (kb.aKey.isPressed) mx -= 1f;
            if (kb.dKey.isPressed) mx += 1f;
            if (kb.sKey.isPressed) mz -= 1f;
            if (kb.wKey.isPressed) mz += 1f;
        }
        if (gamepad != null && Mathf.Approximately(mx, 0f) && Mathf.Approximately(mz, 0f))
        {
            Vector2 ls = gamepad.leftStick.ReadValue();
            mx = ls.x;
            mz = ls.y;
        }
        _inputMove = new Vector2(mx, mz);
        if (_inputMove.sqrMagnitude > 1f) _inputMove.Normalize();

        // Look: use raw mouse delta (pixels). NO deltaTime scaling.
        if (mouse != null)
            _inputLook = mouse.delta.ReadValue();
        else if (gamepad != null)
            _inputLook = gamepad.rightStick.ReadValue() * 5f; // small multiplier for sticks

        // Sprint
        bool shiftHeld = kb != null && kb.leftShiftKey.isPressed;
        if (sprintHold) _isSprinting = shiftHeld;
        else if (kb != null && kb.leftShiftKey.wasPressedThisFrame) _isSprinting = !_isSprinting;

        // Jump
        if (kb != null && kb.spaceKey.wasPressedThisFrame) _wantsJump = true;
        if (gamepad != null && gamepad.aButton.wasPressedThisFrame) _wantsJump = true;

#else
        // Legacy Input Manager
        _inputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_inputMove.sqrMagnitude > 1f) _inputMove.Normalize();

        // Mouse delta: raw, no deltaTime scaling
        _inputLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if (sprintHold) _isSprinting = shift;
        else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            _isSprinting = !_isSprinting;

        if (Input.GetKeyDown(KeyCode.Space)) _wantsJump = true;
#endif
    }

    // -------- Optional external hooks --------
    public void SetSprint(bool sprint) => _isSprinting = sprint;
    public void AddImpulse(Vector3 impulse) => _velocity += impulse; // e.g., knockback
    public void SetLookSensitivity(float sens) => lookSensitivity = Mathf.Max(0.01f, sens);
}
