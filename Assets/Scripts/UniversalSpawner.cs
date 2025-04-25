using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class UniversalSpawner<T> : MonoBehaviour where T : SpawnableObject
{
    [SerializeField] protected SpawnableObject Prefab;
    [SerializeField] protected Platform Ground;
    [SerializeField] protected float RepeatRate = 1.0f;
    [SerializeField] protected int PoolCapacity = 5;
    [SerializeField] protected int PoolMaxSize = 10;
    [SerializeField] protected int SpawnHeight = 7;

    protected ObjectPool<T> Pool;
    private Coroutine _spawnCoroutine;

    public int TotalSpawned { get; private set; }
    public int TotalCreated { get; private set; }
    public int ActiveCount => Pool.CountActive;

    protected virtual void Awake()
    {
        Pool = new ObjectPool<T>(
            createFunc: () =>
            {
                var obj = Instantiate(Prefab).GetComponent<T>();
                obj.TimerEnded += OnTimerEnded;
                TotalCreated++;
                return obj;
            },
            actionOnGet: (obj) =>
            {
                obj.gameObject.SetActive(true);
                obj.transform.position = GetSpawnPosition();
                TotalSpawned++;
            },
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: PoolMaxSize);
    }

    protected virtual void Start()
    {
        if (typeof(T) == typeof(Cube))
            _spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    private void OnDisable()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    protected virtual IEnumerator SpawnObjects()
    {
        WaitForSeconds wait = new(RepeatRate);

        while (enabled)
        {
            Pool.Get();
            yield return wait;
        }
    }

    protected abstract void OnTimerEnded(SpawnableObject obj);

    protected Vector3 GetSpawnPosition()
    {
        Bounds bounds = Ground.GetPlatformRenderer().bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            SpawnHeight,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}