using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
// This component returns the particle system to the pool when the OnParticleSystemStopped event is received.
[RequireComponent(typeof(ParticleSystem))]
public class ReturnToPool : MonoBehaviour
{
    public ParticleSystem _particlesystem;
    public IObjectPool<GameObject> _pool;
   // bool parentObject=false;
    void Start()
    {
       
        _particlesystem = GetComponent<ParticleSystem>();
        var main = _particlesystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    void OnParticleSystemStopped()
    {
        // Return to the pool
        if(_particlesystem != null )    
            _pool.Release(_particlesystem.gameObject);
    }
}
public class ObjectPoolerGeneric : MonoBehaviour
{
    PoolType type;
    public enum PoolType
    {
        Stack,
        LinkedList,
        Particle
    }
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public PoolType poolType;
        public Transform parent;
        public bool preInitialize;
    }
    #region Singleton
    public static ObjectPoolerGeneric Instance;
    public Dictionary<string, IObjectPool<GameObject>> poolDictionary;
    public List<Pool> pools;


    private void SetParticleSystem(ParticleSystem ps,GameObject itemObject,Pool pool) {

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = ps.main;
        main.duration = 1;
        //  main.startLifetime = 1;
        main.loop = false;
        // This is used to return ParticleSystems to the pool when they have stopped.
        var returnToPool = ps.gameObject.AddComponent<ReturnToPool>();
        returnToPool._pool = poolDictionary[pool.tag];
    }
    private void Awake()
        {
      //  DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        poolDictionary = new Dictionary<string, IObjectPool<GameObject>>();
        foreach (Pool pool in pools)
        {
            GameObject CreateItemObject()
            {
                var itemObject = Instantiate(pool.prefab, pool.parent);
                if (pool.poolType == PoolType.Particle) {
                    var ps = itemObject.GetComponent<ParticleSystem>();
                    if (!ps)
                    {
                        ParticleSystem[] psArray = GetComponentsInChildren<ParticleSystem>();
                        if (psArray.Length != 0)
                        {
                            for (int i = 0; i < psArray.Length; i++)
                            {
                                SetParticleSystem(psArray[i], itemObject, pool);
                            }
                        }
                    }
                    else 
                    {
                        SetParticleSystem(ps, itemObject, pool);
                    }
                  


                   
                }
                itemObject.SetActive(false);
                return itemObject;
            }

            void OnGetItem(GameObject item)
            {
                item.gameObject.SetActive(true);
            }

            void OnReleseItem(GameObject item)
            {
              //  GameLog.LogMessage("Releasing Object to Pool:"+ item);
                item.gameObject.SetActive(false);
                item.transform.position= Vector3.zero;
                if (pool.parent && pool.parent!=item.transform.parent)
                    item.transform.SetParent(pool.parent, false);

                item.name = pool.prefab.name + "(Clone)";
                item.transform.localPosition = Vector3.zero;
                
                UI_ItemMain itemMain = item.GetComponent<UI_ItemMain>();
                itemMain?.SetAmountText(0);
                itemMain?.SetItem(null);
                item.transform.localScale = Vector3.one;
                item.transform.localEulerAngles = Vector3.one;
                
              //  GameLog.LogMessage("obj released to pool active flag =" + item.activeSelf, item);
            }

            void OnDestroyPoolObject(GameObject system)
            {
                Destroy(system);
            }

            IObjectPool<GameObject> objectPool;

            if (pool.poolType == PoolType.Stack)
            {
                objectPool = new ObjectPool<GameObject>(CreateItemObject, OnGetItem, OnReleseItem, OnDestroyPoolObject,collectionCheck: false, defaultCapacity: pool.size);
            }
            else if (pool.poolType == PoolType.LinkedList)
            {
                objectPool = new LinkedPool<GameObject>(CreateItemObject, OnGetItem, OnReleseItem);
            }
            else {
                objectPool = new ObjectPool<GameObject>(CreateItemObject, OnGetItem, OnReleseItem, OnDestroyPoolObject, collectionCheck: false, defaultCapacity: pool.size);
            }
            poolDictionary.Add(pool.tag, objectPool);
            if (pool.preInitialize)
             for (int i = 0; i < pool.size; i++)
             {
                var obj = objectPool.Get();
                    obj.SetActive(false);
              }
        }
    }
    #endregion
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
          //  GameLog.LogMessage("Pool with tag " + tag + "doesn't exist in poolDictionary");
            return null;
        }
        GameObject objToSpawn = poolDictionary[tag].Get();
        if (parent != null && parent)
        {
            objToSpawn.transform.SetParent(parent, false);
            objToSpawn.transform.SetAsLastSibling();
        }
      //  GameLog.LogMessage("spawned object " + objToSpawn.name, objToSpawn);

        objToSpawn.SetActive(true);
        if (position!=null)
            objToSpawn.transform.position = position;
        // objToSpawn.transform.anchorPosition;
        if (rotation != null)
            objToSpawn.transform.rotation = rotation;
       
        return objToSpawn;
    }

    public GameObject SpawnFromPool(string tag,bool random, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            GameLog.LogMessage("Pool with tag " + tag + "doesn't exist in poolDictionary");
            return null;
        }
        GameObject objToSpawn = poolDictionary[tag].Get();
       // GameLog.LogMessage("spawned object " + objToSpawn.name, objToSpawn);
        if (parent != null && parent)
        {
            objToSpawn.transform.SetParent(parent, false);
            objToSpawn.transform.SetAsLastSibling();

        }
        objToSpawn.SetActive(true);
        GameLog.LogMessage("spawned object " + objToSpawn.name, objToSpawn);
        objToSpawn.transform.position = parent.position+ Random.insideUnitSphere * 5; 
        // objToSpawn.transform.anchorPosition;
        objToSpawn.transform.rotation = rotation;

        return objToSpawn;
    }
    public void ReleaseToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            GameLog.LogMessage("Pool with tag " + tag + "doesn't exist in poolDictionary");
        }
       // GameLog.LogMessage("obj released to pool active=" + obj.activeSelf, obj);
       
        poolDictionary[tag].Release(obj);
    }
}
