using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalEntity : PoolEntity
{
    public Animator anim;
    public UnityEvent OnAppear;
    public override void Initialize()
    {
        base.Initialize();
        anim.SetTrigger("PopIn");
        OnAppear.Invoke();
    }

    public override void Deactivate()
    {
        StartCoroutine(PopOut());
    }

    /// <summary>
    /// Eventos al desaparecer
    /// </summary>
    /// <returns></returns>
    IEnumerator PopOut()
    {
        anim.SetTrigger("PopOut");
        yield return new WaitForSeconds(1);
        active = false;
        model.transform.localScale = Vector3.zero;
    }

}
