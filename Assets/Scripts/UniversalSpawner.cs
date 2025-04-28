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

    public event Action OnObjectSpawned;
    public event Action OnObjectReleased;
    public event Action OnObjectCreated;

    public int TotalSpawned { get; private set; }
    public int TotalCreated { get; private set; }
    public int ActiveCount => Pool.CountActive;

    public ObjectPool<T> Pool { get; private set; }

    protected virtual void Awake()
    {
        Pool = new ObjectPool<T>(
        createFunc: () =>
        {
            var obj = Instantiate(_prefab).GetComponent<T>();
            obj.TimerEnded += OnTimerEnded;
            TotalCreated++;
            OnObjectCreated?.Invoke();
            return obj;
        },
        actionOnGet: (obj) =>
        {
            obj.gameObject.SetActive(true);
            obj.transform.position = GetSpawnPosition();
            TotalSpawned++;
            OnObjectSpawned?.Invoke();
        },
        actionOnRelease: (obj) =>
        {
            obj.TimerEnded -= OnTimerEnded;
            obj.gameObject.SetActive(false);
            OnObjectReleased?.Invoke();
        },
        actionOnDestroy: (obj) =>
        {
            obj.TimerEnded -= OnTimerEnded;
            Destroy(obj.gameObject);
        },
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
            Pool.Get();
            yield return wait;
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
}