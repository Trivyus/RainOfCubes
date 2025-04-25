using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class UniversalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private Platform _ground;
    [SerializeField] private float _repeatRate = 1.0f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private int _spawnHeight = 7;

    private ObjectPool<Cube> _cubePool;
    private ObjectPool<Bomb> _bombPool;

    private void Awake()
    {
        _cubePool = CreatePool<Cube>(_cubePrefab);
        _bombPool = CreatePool<Bomb>(_bombPrefab);
    }

    private ObjectPool<T> CreatePool<T>(GameObject prefab) where T : SpawnableObject
    {
        return new ObjectPool<T>(
            createFunc: () => 
            {
                var obj = Instantiate(prefab).GetComponent<T>();
                obj.TimerEnded += OnTimerEnded;
                return obj;
            },
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes()
    {
        WaitForSeconds wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            SpawnCube();
            yield return wait;
        }
    }

    private void SpawnCube()
    {
        Cube cube = _cubePool.Get();
        cube.transform.position = GetSpawnPosition();
    }

    private void OnTimerEnded(SpawnableObject obj)
    {
        switch (obj)
        {
            case Cube cube:
                if (cube.gameObject.activeSelf)
                {
                    SpawnBombAt(cube.transform.position);
                    _cubePool.Release(cube);
                }
                break;

            case Bomb bomb:
                if (bomb.gameObject.activeSelf)
                {
                    _bombPool.Release(bomb);
                }
                break;
        }
    }

    private void SpawnBombAt(Vector3 position)
    {
        Bomb bomb = _bombPool.Get();
        bomb.transform.position = position;
    }


    private Vector3 GetSpawnPosition()
    {
        Bounds bounds = _ground.GetPlatformRenderer().bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            _spawnHeight,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}