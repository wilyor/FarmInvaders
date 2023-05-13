using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : PoolEntity
{
    #region VARIABLES
    public string targetTagName = "Player";
    public Transform target;
    public NavMeshAgent nav;
    //gestionarà la màquina de estados
    public Animator anim;
    public float attackDistance = 10;
    public string enemyProjectile = "EnemyProjectile";
    public Transform shootingPoint;
    public float physicalDamage = 10;
    public float attackRadius = 2;

    public UnityEvent OnInitialize;
    public UnityEvent OnDeactivate;

    public bool isMeele = false;
    public bool isBoss = false;
    #endregion

    #region EVENTS
    private void Start()
    {
        SearchTarget(targetTagName);
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDead += PlayerIsDead;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDead -= PlayerIsDead;

    }
    #endregion

    #region METHODS
    /// <summary>
    /// Busca el objetivo más cercano con el tag indicado
    /// </summary>
    public void SearchTarget(string name)
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(name);
        foreach (GameObject possibleTarget in possibleTargets)
        {
            if (possibleTarget)
            {
                target = possibleTarget.transform; 
            }else if (Vector3.Distance(gameObject.transform.position, target.position) >
                Vector3.Distance(gameObject.transform.position, possibleTarget.transform.position))
            {
                target = possibleTarget.transform;
            }
        }
    }

    /// <summary>
    /// revisa si el player está muerto
    /// </summary>
    private void PlayerIsDead()
    {
        anim.Play("Idle");
    }

    /// <summary>
    /// override del initialize
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        OnInitialize.Invoke();
    }

    /// <summary>
    /// Override del Deactivate
    /// </summary>
    public override void Deactivate()
    {
        base.Deactivate();
        OnDeactivate.Invoke();
    }

    /// <summary>
    /// Para hacer un ataque fisico
    /// </summary>
    public void DoPhysicAttack()
    {
        Collider[] impactedObjs = Physics.OverlapSphere(transform.position, attackRadius);
        IDamageable<float> damageable;
        foreach (Collider colliders in impactedObjs)
        {
            damageable = null;
            if (colliders.TryGetComponent<IDamageable<float>>(out damageable))
            {
                if (colliders.CompareTag("Player")) damageable.TakeDamage(physicalDamage, transform.position);
            }
        }
    }


    #endregion
}
