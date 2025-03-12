using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Pool _pool;
    [SerializeField] private GameObject _groundPlane;
    [SerializeField] private float _repeatRate = 1.0f;

    private GetRandom _getRandom = new GetRandom();

    private void Start()
    {
        StartCoroutine(SpawnCube());
    }

    private IEnumerator SpawnCube()
    {
        WaitForSeconds wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            Cube cube = _pool.PoolCubes.Get();
            cube.transform.position = _getRandom.Position(_groundPlane.GetComponent<MeshRenderer>());
            cube.TimerEnded += ReturnCube;
            yield return wait;
        }
    }

    private void ReturnCube(Cube cube)
    {
        cube.TimerEnded -= ReturnCube;
        _pool.PoolCubes.Release(cube);
    }
}