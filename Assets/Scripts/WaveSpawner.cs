using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public Enemy[] enemies;
        public int count;
        public float timeBtwSpawnes;
    }
    public Wave[] waves;
    public Transform[] spawnpoints;
    public float timeBetweenWaves;
    // Start is called before the first frame update
    private Wave currentWave;
    private int currentWaveIndex;
    private Transform player;

   // private bool damaged;
    private Coroutine courutine;

    public float startDelay;

    [SerializeField] ItemsToDamageData relatedItemToDamageData;

    
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        
        relatedItemToDamageData = GetComponent<ItemToDamage>()?.GetItemToDamageData();
        // ObjectPoolerGeneric.Instance.SpawnFromPool("Boss", transform.position, transform.rotation, transform);

        
           
        
    }
    IEnumerator StartNextWaveCourotine(int index)
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(index));
    }
    IEnumerator SpawnWave(int index)
    {
        GameLog.LogMessage("Spawn Wave !!!!!!!!!!!!!!!!!"+index+" spawner name "+transform.name);
        if (index == 0) 
        {
            GameLog.LogMessage("Delay spawning by " + startDelay+ " time="+Time.deltaTime);
            yield return new WaitForSeconds(startDelay);
        }
        GameLog.LogMessage("Continue  spawning b time=" + Time.deltaTime);
        currentWave = waves[index];
        
        for (int i = 0; i < currentWave.count; i++)
        {
            if (player == null)
            {
                yield break;
            }
            if (relatedItemToDamageData !=null && relatedItemToDamageData.damaged)
            {
                StopAllCoroutines();
                yield break;
               

            }

            //  Enemy RandomEnemy = currentWave.enemies[UnityEngine.Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpot = spawnpoints[UnityEngine.Random.Range(0, spawnpoints.Length)];

           // Vector3 randomSpotVector = Random.insideUnitSphere * 10;
            //GameLog.LogMessage("Instantiate Enemy !!!!!!!!!!!!!");
            if (randomSpot)
            {
              
               GameObject enemy =  ObjectPoolerGeneric.Instance.SpawnFromPool("Enemies", transform.position, randomSpot.rotation, randomSpot);
                ObjectPoolerGeneric.Instance.SpawnFromPool("FillEffect", enemy.transform.position, enemy.transform.rotation, enemy.transform);

                 // Instantiate(RandomEnemy, randomSpot.position, randomSpot.rotation);
                 yield return new WaitForSeconds(currentWave.timeBtwSpawnes);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (relatedItemToDamageData && relatedItemToDamageData.damaged)
                return;

            if (courutine==null)
                courutine = StartCoroutine(StartNextWaveCourotine(currentWaveIndex));
        }
    }

    private void OnBecameVisible()
    {
        if (relatedItemToDamageData && relatedItemToDamageData.damaged)
            StopAllCoroutines();
        else
            courutine = StartCoroutine(StartNextWaveCourotine(currentWaveIndex));

       
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
