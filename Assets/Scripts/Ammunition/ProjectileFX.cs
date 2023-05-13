using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileFX : MonoBehaviour
{
    public UnityEvent onInitialize;
    public UnityEvent<Vector3> onImpact;
    //projectile del cual escuchare los eventos
    public Projectile projectile;

    private void OnEnable()
    {
        //nos suscribimos al action de impacto con el invoke del unity event. ambos coinciden con el parametro con el que se crearon Vector3
        //de no coincidri, no se podría suscribir
        projectile.OnImpact += onImpact.Invoke;
        projectile.OnInitialize += onInitialize.Invoke;
    }

    private void OnDestroy()
    {
        projectile.OnImpact -= onImpact.Invoke;
        projectile.OnInitialize -= onInitialize.Invoke;
    }
}
