using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate_Spawner : MonoBehaviour
{
    public GameObject[] plates;
    public Vector3 spawnValues;
    public float spawnWait;
    public float spawnLeastWait;
    public float spawnMostWait;
    public int startWait;
    public bool stop;
    public float checkRadius = 1f; // Radius to check for overlaps

    int randPlate;

    void Start()
    {
       StartCoroutine(waitSpawner()); 
    }

    void Update()
    {
        spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
    }

    IEnumerator waitSpawner()
    {
        yield return new WaitForSeconds(startWait);

        while (!stop)
        {
            randPlate = Random.Range(0, plates.Length);
            Vector3 spawnPosition;

            // Find a valid spawn position
            do
            {
                spawnPosition = new Vector3(
                    Random.Range(-spawnValues.x, spawnValues.x),
                    0.1f,
                    Random.Range(-spawnValues.z, spawnValues.z)
                );
            } 
            while (IsPositionOccupied(spawnPosition));

            Instantiate(plates[randPlate], spawnPosition + transform.TransformPoint(Vector3.zero), gameObject.transform.rotation);

            yield return new WaitForSeconds(spawnWait);
        }
    }

    // Check if there's an overlap at the spawn position
    bool IsPositionOccupied(Vector3 spawnPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, checkRadius);
        return colliders.Length > 0; // True if there is an overlap
    }
}
