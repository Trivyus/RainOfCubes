using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private Platform _ground;
    [SerializeField] private float _repeatRate = 1.0f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private int _spawnHeight = 7;

    private ObjectPool<Cube> _poolCubes;

    private void Awake()
    {
        InstantiatePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private void InstantiatePool()
    {
        _poolCubes = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cube),
            actionOnGet: (cube) => GetFromPool(cube),
            actionOnRelease: (cube) => ReleaseInPool(cube),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private IEnumerator SpawnCubes()
    {
        WaitForSeconds wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            Cube cube = _poolCubes.Get();
            cube.transform.position = GetSpawnPosition();
            cube.TimerEnded += ReturnCube;
            yield return wait;
        }
    }

    private void GetFromPool(Cube cube)
    {
        cube.gameObject.SetActive(true);
    }

    private void ReleaseInPool(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void ReturnCube(Cube cube)
    {
        cube.TimerEnded -= ReturnCube;
        _poolCubes.Release(cube);
    }

    private Vector3 GetSpawnPosition()
    {
        Bounds bounds = _ground.GetPlatformRenderer().bounds;

        return new Vector3 { x = Random.Range(bounds.min.x, bounds.max.x), y = _spawnHeight, z = Random.Range(bounds.min.z, bounds.max.z) };
    }
}