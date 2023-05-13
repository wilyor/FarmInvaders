using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldCollectable : PoolEntity, ICollectable
{
    public static Action OnCollected;
    public UnityEvent OnGathered;
    /// <summary>
    /// Action when collected
    /// </summary>
    public void OnCollect(PlayerShooting shooting)
    {
        shooting.gameObject.GetComponent<PlayerHealth>().ActivateShield();
        OnCollected?.Invoke();
        ReturnToPool();
        OnGathered.Invoke();

    }

}
