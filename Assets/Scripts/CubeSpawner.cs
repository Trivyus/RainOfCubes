using UnityEngine;

public class CubeSpawner : UniversalSpawner<Cube>
{
    [SerializeField] private BombSpawner _bombSpawner;

    protected override void Awake()
    {
        base.Awake();
        StartSpawning();
    }

    protected override void OnTimerEnded(Cube cube)
    {
        if (cube.gameObject.activeSelf)
        {
            _bombSpawner.SpawnAtPosition(cube.transform.position);
            Pool.Release(cube);
        }
    }
}
