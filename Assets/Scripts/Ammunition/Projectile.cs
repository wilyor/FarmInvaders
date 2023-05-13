using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Para poder usar los action events
using System;
using UnityEngine.Events;

public class Projectile : PoolEntity
{
    #region VARIABLES

    public Transform shooterPosition;
    [Header("Components")]
    public Collider col;
    public Rigidbody rb;
    public ParticleSystem trail;

    [Header("Projectile")]
    public float damage = 10;
    public float speed =10;
    public float lifeTime = 5;
    protected float lifeTimeStamp;
    public LayerMask shooteableLayer;

    [Header("Events")]
//Action que informara cuando el proyectil haga impacto
    public Action<Vector3> OnImpact;
    public Action OnInitialize;
    public UnityEvent OnShoot;
    #endregion

    #region EVENTS
    void Update()
    {
        if (lifeTimeStamp < Time.time && active) ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        //comprobando si el layer del objeto impactado se encuentra dentro de nuestro layer mask
        if((shooteableLayer & (1 << other.gameObject.layer)) != 0)
        {
            GenerateDamage(other);
            OnImpact?.Invoke(transform.position);
            ReturnToPool();
        }
    }

    /// <summary>
    /// Forma de causar daño
    /// </summary>
    /// <param name="col"></param>
    public virtual void GenerateDamage(Collider col)
    {
        IDamageable<float> damageable;
        if (col.TryGetComponent<IDamageable<float>>(out damageable))
        {
            damageable.TakeDamage(damage, transform.position);

        }
        if (col.CompareTag("Destroyable")) col.GetComponent<Destroyable>().Destroy();

    }
    #endregion

    #region METHODS
    /// <summary>
    /// Para recuperar los componentes necesarios
    /// </summary>
    [ContextMenu("Initialize Components")]
    public void GetComponents()
    {
        col = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Inicializa los componentes de la entidad pooleable
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        OnInitialize?.Invoke();
        col.enabled = true;
        rb.isKinematic = false;
        trail.Play();
        lifeTimeStamp = Time.time + lifeTime;
        Shoot();
    }

    /// <summary>
    /// Eventos al disparar
    /// </summary>
    public virtual void Shoot()
    {
        OnShoot.Invoke();
        rb.velocity = transform.forward * speed;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        col.enabled = false;
        rb.isKinematic = true;
        trail.Stop();
        rb.velocity = Vector3.zero;
    }
    #endregion
}
