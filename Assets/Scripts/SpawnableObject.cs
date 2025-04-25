using System;
using UnityEngine;

public abstract class SpawnableObject : MonoBehaviour
{
    public event Action<SpawnableObject> TimerEnded;

    protected void NotifyTimerEnded()
    {
        TimerEnded?.Invoke(this);
    }
}
