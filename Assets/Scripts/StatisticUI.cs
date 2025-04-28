using System;
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

    private void OnEnable()
    {
        SubscribeToSpawner(_cubeSpawner, UpdateCubeStats);
        SubscribeToSpawner(_bombSpawner, UpdateBombStats);

        UpdateCubeStats();
        UpdateBombStats();
    }

    private void OnDisable()
    {
        UnsubscribeFromSpawner(_cubeSpawner, UpdateCubeStats);
        UnsubscribeFromSpawner(_bombSpawner, UpdateBombStats);
    }

    private void SubscribeToSpawner<T>(UniversalSpawner<T> spawner, Action updateAction)
        where T : SpawnableObject<T>
    {
        if (spawner != null)
        {
            spawner.OnObjectSpawned += updateAction;
            spawner.OnObjectReleased += updateAction;
            spawner.OnObjectCreated += updateAction;
        }
    }

    private void UnsubscribeFromSpawner<T>(UniversalSpawner<T> spawner, Action updateAction)
        where T : SpawnableObject<T>
    {
        if (spawner != null)
        {
            spawner.OnObjectSpawned -= updateAction;
            spawner.OnObjectReleased -= updateAction;
            spawner.OnObjectCreated -= updateAction;
        }
    }

    private void UpdateCubeStats()
    {
        UpdateStats(_cubeSpawner, _cubeTotalSpawned, _cubeTotalCreated, _cubeActiveCount, "Cube");
    }

    private void UpdateBombStats()
    {
        UpdateStats(_bombSpawner, _bombTotalSpawned, _bombTotalCreated, _bombActiveCount, "Bomb");
    }

    private void UpdateStats<T>(UniversalSpawner<T> spawner, TMP_Text totalSpawnedText,
                             TMP_Text totalCreatedText, TMP_Text activeCountText, string objectName)
        where T : SpawnableObject<T>
    {
        if (spawner == null || totalSpawnedText == null ||
            totalCreatedText == null || activeCountText == null)
            return;

        totalSpawnedText.text = $"Total {objectName}s Spawned: {spawner.TotalSpawned}";
        totalCreatedText.text = $"Total {objectName}s Created: {spawner.TotalCreated}";
        activeCountText.text = $"Active {objectName}s: {spawner.ActiveCount}";
    }
}
