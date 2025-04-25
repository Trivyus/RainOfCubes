using UnityEngine;

public class BombSpawner : UniversalSpawner<Bomb>
{
    public void SpawnAtPosition(Vector3 position)
    {
        var bomb = Pool.Get();
        bomb.transform.position = position;
    }

    protected override void OnTimerEnded(SpawnableObject obj)
    {
        if (obj is Bomb bomb && bomb.gameObject.activeSelf)
        {
            Pool.Release(bomb);
        }
    }
}
