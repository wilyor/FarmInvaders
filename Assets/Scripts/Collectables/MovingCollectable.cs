using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingCollectable : MonoBehaviour
{
    public float targetArea = 4f;
    public float staticWaiting = 2f;
    public NavMeshAgent nav;
    public Animator anim;
    private bool walking = false;
    public Vector3 nextTarget;
    private Coroutine waitingCoroutine;
    private float maxTimeWalking = 5;
    private float currentTimeWalking = 5;

    private void Update()
    {
        currentTimeWalking -= Time.deltaTime;
        if ((walking && Vector3.Distance(transform.position, nextTarget) <= 0.5f) || (walking && currentTimeWalking <= 0))
        {
            StopMoving();
        }
    }

    /// <summary>
    /// Genera aleatoriamente un punto comoo proximo target a moverse
    /// </summary>
    public void SearchTarget()
    {
        nextTarget = new Vector3(Random.Range(1, targetArea), 0, Random.Range(1, targetArea)) + transform.position;
        nav.SetDestination(nextTarget);
        walking = true;
        anim.SetBool("Walking", walking);
    }

    /// <summary>
    /// acciones a hacer cuando se detenga el movimiento
    /// </summary>
    public void StopMoving()
    {
        Dissapear();
        waitingCoroutine = StartCoroutine(WaitForNextMove());
    }

    /// <summary>
    /// Actions when dissapearing
    /// </summary>
    public void Dissapear()
    {
        walking = false;
        anim.SetBool("Walking", walking);
        currentTimeWalking = maxTimeWalking;
        if (waitingCoroutine != null) StopCoroutine(waitingCoroutine);

    }
    /// <summary>
    /// espera para buscar un nuevo punto de movimiento
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForNextMove()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, staticWaiting));
        SearchTarget();
    }
}
