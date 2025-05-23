using System;
using UnityEngine;

public abstract class SpawnableObject<T> : MonoBehaviour where T : SpawnableObject<T>
{
    public event Action<T> TimerEnded;

    public virtual void Init(){ }

    protected void NotifyTimerEnded()
    {
        TimerEnded?.Invoke((T)this);
    }
}
