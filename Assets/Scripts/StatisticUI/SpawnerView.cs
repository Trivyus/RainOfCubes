using TMPro;
using UnityEngine;

public abstract class SpawnerView<T> : MonoBehaviour where T : SpawnableObject<T>
{
    [SerializeField] private UniversalSpawner<T> _spawner;
    [SerializeField] private TMP_Text _totalSpawnedText;
    [SerializeField] private TMP_Text _totalCreatedText;
    [SerializeField] private TMP_Text _activeCountText;

    protected abstract string ObjectName { get; }

    private void OnEnable()
    {
        if (_spawner != null)
        {
            _spawner.ObjectSpawned += UpdateStats;
            _spawner.ObjectReleased += UpdateStats;
            _spawner.ObjectCreated += UpdateStats;
        }

        UpdateStats();
    }

    private void OnDisable()
    {
        if (_spawner != null)
        {
            _spawner.ObjectSpawned -= UpdateStats;
            _spawner.ObjectReleased -= UpdateStats;
            _spawner.ObjectCreated -= UpdateStats;
        }
    }

    protected virtual void UpdateStats()
    {
        if (_spawner == null || _totalSpawnedText == null ||
            _totalCreatedText == null || _activeCountText == null)
            return;

        _totalSpawnedText.text = $"Total {ObjectName}s Spawned: {_spawner.TotalSpawned}";
        _totalCreatedText.text = $"Total {ObjectName}s Created: {_spawner.TotalCreated}";
        _activeCountText.text = $"Active {ObjectName}s: {_spawner.ActiveCount}";
    }
}