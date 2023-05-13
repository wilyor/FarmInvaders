using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParaboleProjectile : Projectile
{
    #region VARIABLES
    public float damageRatio = 6f;
    private Transform aim;

    public float lifeCounter = 0;
    public UnityEvent OnDeactivate;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public Vector3 initialShooterPosition;

    #endregion

    #region EVENTS

    private void Update()
    {
        if (lifeCounter < -1 && active) ReturnToPool();
        if (active)
        {
            transform.position = Vector3.Slerp(startPosition - initialShooterPosition
                , targetPosition - initialShooterPosition
                , 1 - lifeCounter / lifeTime ) + initialShooterPosition;
            lifeCounter -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRatio);
    }
    #endregion

    #region METHODS
    public override void Initialize()
    {
        base.Initialize();
        aim = GameObject.Find("Tank").GetComponent<PlayerShooting>().aim;
        startPosition = transform.position;
        lifeCounter = lifeTime;
        OnInitialize?.Invoke();
        targetPosition = aim.transform.position;
        initialShooterPosition = shooterPosition ? shooterPosition.position : Vector3.zero;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        OnDeactivate?.Invoke();
    }

    public override void GenerateDamage(Collider col)
    {
        Collider[] impactedObjs = Physics.OverlapSphere(transform.position, damageRatio);
        IDamageable<float> damageable;
        foreach (Collider colliders in impactedObjs)
        {
            damageable = null;
            if(colliders.TryGetComponent<IDamageable<float>>(out damageable))
            {
                if(!colliders.CompareTag("Player")) damageable.TakeDamage(damage, transform.position);
            }
            if (col.CompareTag("Destroyable")) col.GetComponent<Destroyable>().Destroy();
        }
    }
    #endregion
}
