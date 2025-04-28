using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class UniversalSpawner<T> : MonoBehaviour where T : SpawnableObject<T>
{
    [SerializeField] private SpawnableObject<T> _prefab;
    [SerializeField] private Platform _ground;
    [SerializeField] private float _repeatRate = 1.0f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private int _spawnHeight = 7;

    private Coroutine _spawnCoroutine;

    public event Action ObjectSpawned;
    public event Action ObjectReleased;
    public event Action ObjectCreated;

    public int TotalSpawned { get; private set; }
    public int TotalCreated { get; private set; }
    public int ActiveCount => _pool.CountActive;

    private ObjectPool<T> _pool;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
             createFunc: CreateObject,
             actionOnGet: ActionOnGetObject,
             actionOnRelease: ActionOnReleaseObject,
             actionOnDestroy: ActionOnDestroyObject,
             collectionCheck: true,
             defaultCapacity: _poolCapacity,
             maxSize: _poolMaxSize);
    }

    private void OnDisable()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    public void StartSpawning()
    {
        _spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    protected virtual IEnumerator SpawnObjects()
    {
        WaitForSeconds wait = new(_repeatRate);

        while (enabled)
        {
            _pool.Get();
            yield return wait;
        }
    }

    protected T GetObjectFromPool()
    {
        return _pool.Get();
    }

    protected void ReleaseObjectToPool(T obj)
    {
        if (obj.gameObject.activeSelf)
        {
            _pool.Release(obj);
        }
    }

    protected abstract void OnTimerEnded(T obj);

    protected Vector3 GetSpawnPosition()
    {
        Bounds bounds = _ground.GetPlatformRenderer().bounds;
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            _spawnHeight,
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private T CreateObject()
    {
        var obj = Instantiate(_prefab).GetComponent<T>();
        obj.TimerEnded += OnTimerEnded;
        TotalCreated++;
        ObjectCreated?.Invoke();
        return obj;
    }

    private void ActionOnGetObject(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.Init();
        obj.transform.position = GetSpawnPosition();
        TotalSpawned++;
        ObjectSpawned?.Invoke();
    }

    private void ActionOnReleaseObject(T obj)
    {
        obj.TimerEnded -= OnTimerEnded;
        obj.gameObject.SetActive(false);
        ObjectReleased?.Invoke();
    }

    private void ActionOnDestroyObject(T obj)
    {
        obj.TimerEnded -= OnTimerEnded;
        Destroy(obj.gameObject);
    }
}