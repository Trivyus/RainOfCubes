using UnityEngine;

public class CubeSpawner : UniversalSpawner<Cube>
{
    [SerializeField] private BombSpawner _bombSpawner;

    protected override void OnTimerEnded(SpawnableObject obj)
    {
        if (obj is Cube cube && cube.gameObject.activeSelf)
        {
            _bombSpawner.SpawnAtPosition(cube.transform.position);
            Pool.Release(cube);
        }
    }
}
