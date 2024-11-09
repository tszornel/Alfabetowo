using UnityEngine;
using System.Collections;
public class SpawnerPoint : MonoBehaviour
{
    private float nextSpawn = 0;
    public Transform prefabToSpawn;
    public float spawnRate = 5f;
    public float randomDelay = 10f;
    void Start()
    {
    }
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            nextSpawn = Time.time + spawnRate + Random.Range(4, randomDelay);
           // GameLog.LogMessage("nextspawn" + nextSpawn);
        }
    }
}