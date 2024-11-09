using System.Text;
using UnityEngine;
using UnityEngine.Pool;

// This component returns the particle system to the pool when the OnParticleSystemStopped event is received.
[RequireComponent(typeof(ParticleSystem))]
 class ReturnToPoolNew : MonoBehaviour
{
    public ParticleSystem system;
    public IObjectPool<ParticleSystem> pool;

    void Start()
    {
        system = GetComponent<ParticleSystem>();
        var main = system.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        // Return to the pool
        pool.Release(system);
    }
}

// This example spans a random number of ParticleSystems using a pool so that old systems can be reused.
public class ParticlePool : MonoBehaviour
{
    public enum PoolType
    {
        Stack,
        LinkedList
    }

    public PoolType poolType;

    //
    // ion checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 100;

    IObjectPool<ParticleSystem> m_Pool;
    [SerializeField] ParticleSystem _particlePrefab;
    [SerializeField] Transform _particlePrefabParent;

    #region Singleton
    public static ParticlePool Instance;
    private void Awake()
    {
        Instance = this;

    }
    #endregion

    public IObjectPool<ParticleSystem> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                if (poolType == PoolType.Stack)
                    m_Pool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                else
                    m_Pool = new LinkedPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
            }
            return m_Pool;
        }
    }

    ParticleSystem CreatePooledItem()
    {
        //var go = new GameObject("Pooled Particle System");
        ParticleSystem go;
        if (_particlePrefabParent)
        {
             go = Instantiate(_particlePrefab, _particlePrefabParent);
           
        }

        else {
             go = Instantiate(_particlePrefab);
           

        }

        var ps = go.GetComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 1;
        main.startLifetime = 1;
        main.loop = false;

        // This is used to return ParticleSystems to the pool when they have stopped.
        var returnToPool = go.gameObject.AddComponent<ReturnToPoolNew>();
        returnToPool.pool = Pool;

        return ps;
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(ParticleSystem system)
    {
        system.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(ParticleSystem system)
    {
        if (system && system.gameObject)
            system.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(ParticleSystem system)
    {
        Destroy(system.gameObject);
    }

   /* void OnGUI()
    {

      //  GameLog.LogMessage("ON GUI ENTERED ");
        GUILayout.Label("Pool size: " + Pool.CountInactive);
        if (GUILayout.Button("Create Particles"))
        {
          //  GameLog.LogMessage("GUI BUTTON IF ON GUI ENTERED ");
            var amount = Random.Range(1, 10);
            for (int i = 0; i < amount; ++i)
            {
                var ps = Pool.Get();
                if (ps)
                {
                    ps.transform.position = Random.insideUnitSphere * 10;
                    ps.Play();
                }
                else {

                    GameLog.LogMessage("particle get failed ??");
                }
            }
        }
    }
   */
}