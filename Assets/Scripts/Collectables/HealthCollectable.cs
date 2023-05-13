using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthCollectable : EvolvingAmmunition
{
    public int cureValue = 10;
    public static Action OnCollected;
    /// <summary>
    /// Action when collected
    /// </summary>
    public override void OnCollect(PlayerShooting shooting)
    {
        shooting.gameObject.GetComponent<PlayerHealth>().Cure(cureValue);
        OnCollected?.Invoke();
        HideElement();
        OnGathered.Invoke();

    }
}
