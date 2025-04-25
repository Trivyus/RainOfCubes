using TMPro;
using UnityEngine;

public class StatisticUI : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;

    [Header("Cube Stats")]
    [SerializeField] private TMP_Text _cubeTotalSpawned;
    [SerializeField] private TMP_Text _cubeTotalCreated;
    [SerializeField] private TMP_Text _cubeActiveCount;

    [Header("Bomb Stats")]
    [SerializeField] private TMP_Text _bombTotalSpawned;
    [SerializeField] private TMP_Text _bombTotalCreated;
    [SerializeField] private TMP_Text _bombActiveCount;

    private void Update()
    {
        UpdateCubeStats();
        UpdateBombStats();
    }

    private void UpdateCubeStats()
    {
        _cubeTotalSpawned.text = $"Total cubes Spawned: {_cubeSpawner.TotalSpawned}";
        _cubeTotalCreated.text = $"Total cubes Created: {_cubeSpawner.TotalCreated}";
        _cubeActiveCount.text = $"Active cubes: {_cubeSpawner.ActiveCount}";
    }

    private void UpdateBombStats()
    {
        _bombTotalSpawned.text = $"Total bomb Spawned: {_bombSpawner.TotalSpawned}";
        _bombTotalCreated.text = $"Total bomb Created: {_bombSpawner.TotalCreated}";
        _bombActiveCount.text = $"Active bomb: {_bombSpawner.ActiveCount}";
    }
}
