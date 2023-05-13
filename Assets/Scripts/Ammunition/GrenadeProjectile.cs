using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrenadeProjectile : Projectile
{
    #region VARIABLES
    public float damageRatio = 4f;
    public float forceY = 3f;
    private Transform aim;
    public Animator anim;

    public float lifeCounter = 0;
    public UnityEvent OnDeactivate;
    public UnityEvent OnExplosion;

    public bool isFromTank = true;
    #endregion

    #region EVENTS

    private void Awake()
    {
        aim = GameObject.Find("Tank").GetComponent<PlayerShooting>().aim;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((shooteableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            GenerateDamage();
            OnImpact?.Invoke(transform.position);
            ReturnToPool();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            if(anim != null) anim.SetTrigger("Explote");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRatio);
    }

    #endregion

    public override void Initialize()
    {
        base.Initialize();
        if(isFromTank) aim = GameObject.Find("Tank").GetComponent<PlayerShooting>().aim;
        lifeCounter = lifeTime;
        OnInitialize?.Invoke();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        OnDeactivate?.Invoke();
    }

    public override void Shoot()
    {
        if (isFromTank) rb.AddForce(CalculateParabole(), ForceMode.Impulse);
    }

    public override void GenerateDamage(Collider col = null)
    {
        Collider[] impactedObjs = Physics.OverlapSphere(transform.position, damageRatio);
        IDamageable<float> damageable;
        foreach (Collider colliders in impactedObjs)
        {
            damageable = null;
            if (colliders.TryGetComponent<IDamageable<float>>(out damageable))
            {
                if (!colliders.CompareTag("Player")) damageable.TakeDamage(damage, transform.position);
            }

            if (colliders.CompareTag("Destroyable")) colliders.GetComponent<Destroyable>().Destroy() ;
        }
    }
    /// <summary>
    /// Calcula la fuerza para la parabola
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateParabole()
    {
        float force = Vector3.Distance(transform.position, aim.position);
        return transform.forward * force + new Vector3(0f, forceY, 0f);
    }

    /// <summary>
    /// Acciones cuando explota
    /// </summary>
    public void Explote()
    {
        if (!active) return;
        OnImpact?.Invoke(transform.position);
        GenerateDamage();
        ReturnToPool();
    }
}
