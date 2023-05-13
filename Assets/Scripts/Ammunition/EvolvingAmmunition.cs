using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvolvingAmmunition : PoolEntity, ICollectable
{

    public BulletType bulletType;
    #region VARIABLES
    [Header("Components")]
    public Collider col;

    [Header("Evolution")]
    public float timeToEvolve = 5f;
    private float nextEvolutionTime = 0;
    private float currentEvolutiontime = 0;
    public string nextEvolutionableObjectId;
    public ParticleSystem evolvingParticles;
    public ProgressBarManager evolutionBar;
    public UnityEvent OnInitialize;
    public UnityEvent OnDeactivate;
    public UnityEvent OnGathered;
    public string code;
    public static Action <string> OnRecollected;
    #endregion

    #region EVENTS

    private void Start()
    {
        if (evolutionBar)
        {
            evolutionBar.InitializeBar(timeToEvolve);
        }
    }
    void Update()
    {
        CheckEvolutionTime();
    }

    #endregion

    #region METHODS
    /// <summary>
    /// Revisa el tiempo de evollución y manda esta info a la barra de evolucion
    /// </summary>
    private void CheckEvolutionTime()
    {
        if (active)
        {
            currentEvolutiontime += Time.deltaTime;
            evolutionBar.SetValue(currentEvolutiontime);
            if (nextEvolutionTime < Time.time)
            {
                Evolve();
            }
        }
    }
    /// <summary>
    /// Transforma el elemento actual en otro
    /// </summary>
    public void Evolve()
    {
        HideElement();
        if (nextEvolutionableObjectId != "")
        {
            PoolEntity newEntity = PoolsManager.instance.PullElement(nextEvolutionableObjectId, transform.position, transform.rotation);
            CollectableGenerator.instance.AddEntityToList(newEntity);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        col.enabled = true;
        nextEvolutionTime = Time.time + timeToEvolve;
        currentEvolutiontime = 0;
        OnInitialize.Invoke();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        col.enabled = false;
        OnDeactivate.Invoke();
    }

    /// <summary>
    /// esconde el objeto y deja un rastro de particulas
    /// </summary>
    public void HideElement()
    {
        ReturnToPool();
        evolvingParticles.Play();
    }

    /// <summary>
    /// Action when collected
    /// </summary>
    public virtual void OnCollect(PlayerShooting shooting)
    {
        shooting.AddBullets(bulletType, 1);
        HideElement();
        OnRecollected?.Invoke(code);
        OnGathered.Invoke();
    }
    #endregion

}
