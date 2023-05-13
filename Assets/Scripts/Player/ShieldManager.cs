using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldManager : MonoBehaviour
{
    public UnityEvent OnShieldDeactivate;
    public Animator anim;
    public float deactivationTime = 10;
    public ProgressBarManager shieldBar;
    float currentTime = 0;

    private void OnEnable()
    {
        ActivateShield();
    }

    private void Update()
    {
        CheckTime();
    }

    /// <summary>
    /// Acciones cada momento
    /// </summary>
    private void CheckTime()
    {
        if (currentTime <= 0)
        {
            StartCoroutine(ShieldDeactivation());
        }
        else
        {
            shieldBar.SetValue(currentTime);
        }
        currentTime -= Time.deltaTime;
    }
    /// <summary>
    /// Acciones al activar el escudo
    /// </summary>
    private void ActivateShield()
    {
        if (anim == null) anim = GetComponent<Animator>();
        anim.SetTrigger("Activate");
        currentTime = deactivationTime;
        shieldBar.InitializeBar(deactivationTime);
    }

    /// <summary>
    /// Desactiva el escudo despues de x tiempo
    /// </summary>
    /// <returns></returns>
    IEnumerator ShieldDeactivation()
    {
        if (anim == null) anim = GetComponent<Animator>();
        anim.SetTrigger("Deactivate");
        yield return new WaitForSeconds(0.5f);
        OnShieldDeactivate.Invoke();
        gameObject.SetActive(false);
    }
}
