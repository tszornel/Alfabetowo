using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class ObjectPooler : MonoBehaviour
{
    public enum PoolType
    {
        Stack,
        LinkedList, 
        Queue
    }
    [System.Serializable]
    public class Pool 
    { 
       public string tag;
        public GameObject prefab;
        public int size;
        public PoolType poolType;
        public Transform parent;
    }
    // IObjectPool<Transform> slotPool = new ObjectPool<Transform>(CreateItemObject, OnGetItem, OnReleseItem, defaultCapacity: 3);
    #region Singleton
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj;
                if (!pool.parent)
                    obj = Instantiate(pool.prefab);
                else
                    obj = Instantiate(pool.prefab, pool.parent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    #endregion
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;
    // Start is called before the first frame update
    void Start()
    {
    }
    public GameObject SpawnFromPool(string tag,Vector3 position,Quaternion rotation, Transform parent) 
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            GameLog.LogMessage("Pool with tag " + tag + "doesn't exist in poolDictionary");
            return null;
        }
            GameObject objToSpawn = poolDictionary[tag].Dequeue();
            objToSpawn.SetActive(true);
             objToSpawn.transform.position = position;
        // objToSpawn.transform.anchorPosition;
            objToSpawn.transform.rotation = rotation;
        if (parent) {
            objToSpawn.transform.SetParent(parent, false);
            objToSpawn.transform.SetAsLastSibling();
        }
        return objToSpawn;
    }
    public GameObject SpawnFromPoolAutomaticBack(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            GameLog.LogMessage("Pool with tag " + tag + "doesn't exist in poolDictionary");
            return null;
        }
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        // objToSpawn.transform.anchorPosition;
        objToSpawn.transform.rotation = rotation;
        if (parent)
        {
            objToSpawn.transform.SetParent(parent, false);
            objToSpawn.transform.SetAsLastSibling();
        }
        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
    public void ReleaseToPool(string tag,GameObject obj) {
        if (!poolDictionary.ContainsKey(tag))
        {
            GameLog.LogMessage("Pool with tag " + tag + "doesn't exist in poolDictionary");
        }
        foreach (Pool pool in pools)
        {
            if (tag == pool.tag) {
                GameLog.LogMessage("Set Pools Parent" + pool.parent,obj);
                obj.transform.SetParent(pool.parent,false);
            }
        }
        obj.SetActive(false);
        GameLog.LogMessage("obj released to pool active="+obj.activeSelf, obj);
        poolDictionary[tag].Enqueue(obj);
    }
}
