//using UnityEngine;

//public class CounterStackSpawner : MonoBehaviour
//{
//    [System.Serializable]
//    public class StackSlot
//    {
//        public string plateType;
//        public Transform spawnPoint;
//        [HideInInspector] public GameObject currentStack;
//    }

//    public GameObject stackPrefab;
//    public StackSlot[] stackSlots;

//    void Start()
//    {
//        CheckAndSpawnStacks();
//    }

//    public void CheckAndSpawnStacks()
//    {
//        foreach (var slot in stackSlots)
//        {
//            // If there's already a valid stack of the correct type, skip
//            if (slot.currentStack != null)
//            {
//                StackManager existing = slot.currentStack.GetComponent<StackManager>();
//                if (existing != null && existing.plateType == slot.plateType)
//                    continue;
//            }

//            // Otherwise, spawn a new stack
//            GameObject newStack = Instantiate(stackPrefab, slot.spawnPoint.position, Quaternion.identity);
//            newStack.transform.SetParent(slot.spawnPoint); // Optional: parent to keep things tidy

//            StackManager manager = newStack.GetComponent<StackManager>();
//            manager.plateType = slot.plateType;

//            slot.currentStack = newStack;
//        }
//    }
//}
