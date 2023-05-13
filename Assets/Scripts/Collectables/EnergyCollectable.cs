using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyCollectable : PoolEntity, ICollectable
{
    public float speed = 50f;
    public float energyValue = 10;
    public UnityEvent OnGathered;

    private void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    /// <summary>
    /// Action when collected
    /// </summary>
    public void OnCollect(PlayerShooting shooting)
    {
        GameObject tank = GameObject.Find("tank");
        shooting.AddPowerToSpecial(energyValue);
        OnGathered.Invoke();
        HideElement();
    }

    /// <summary>
    /// esconde el objeto
    /// </summary>
    public void HideElement()
    {
        Destroy(this.gameObject);
    }
}
