using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable<float>
{

    #region VARIABLES
    public float startingHealth = 20;
    public float currentHealth;
    public Animator anim;
    public static Action OnDead;
    public static Action OnBossDead;
    public UnityEvent OnDeadEvent;

    [Header("Renderer")]
    public Renderer[] renderers;
    //Para no generar instancias de materiales
    private MaterialPropertyBlock dissolvePropertyBlock;
    public float dissolveTime = 5f;
    //Altura m�xima y minima para la disoluci�n
    public float dissolveMaxHeight = 2f;
    public float dissolveMinHeight = -2f;
    private float dissolveCounter;
    private Coroutine dissolveCoroutine;
    //Evento de unity para cuando se haya disuelto cmpletamente el enemigo
    public UnityEvent OnDissolve;
    public UnityEvent OnDamaged;

    public ProgressBarManager healthBar;
    public Coroutine healthBarCoroutine;
    #endregion
    #region EVENTS

    private void Awake()
    {
        dissolvePropertyBlock = new MaterialPropertyBlock();
        renderers = GetComponentsInChildren<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Revive();
    }
    #endregion

    #region METHODS

    /// <summary>
    /// PAra saber si el enemigo est� muerto
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    /// <summary>
    /// acciones cuando el jugador toma da�o
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="impact"></param>
    public void TakeDamage(float damage, Vector3 impact = default)
    {
        if (IsDead()) return;
        currentHealth -= damage;
        RenderHealthBar();
        if (IsDead())
        {
            Die();
        }
    }

    /// <summary>
    /// Acciones necesarias para la muerte del enemigo
    /// </summary>
    private void Die()
    {
        if (anim) anim.SetTrigger("Death");
        OnDeadEvent?.Invoke();
        if (GetComponent<Enemy>().isBoss)
        {
            OnBossDead?.Invoke();
        }
        else
        {
            OnDead?.Invoke();
        }

        if (dissolveCoroutine != null) StopCoroutine(dissolveCoroutine);
        dissolveCoroutine = StartCoroutine(DissolveCoroutine());
        if (healthBar)
        {
            healthBar.enabled = false;
        }
    }

    /// <summary>
    /// Restaura la vida al enemigo
    /// </summary>
    public void Revive()
    {
        currentHealth = startingHealth;

        if (dissolveCoroutine != null) StopCoroutine(dissolveCoroutine);

        if (anim)
        {
            anim.ResetTrigger("Dead");
            anim.Play("Forward");
        }

        ///Recuperar el property block para modificarlo y posteriormente volverlo a poner
        if (renderers.Length > 0)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.GetPropertyBlock(dissolvePropertyBlock);
                dissolvePropertyBlock.SetFloat("_CutoffHeight", dissolveMaxHeight);
                renderer.SetPropertyBlock(dissolvePropertyBlock);
            }
        }

        if (healthBar != null)
        {
            healthBar.InitializeBar(startingHealth);
            healthBar.enabled = false;
        }
    }

    /// <summary>
    /// Coroutina para hacer la disoluci�n
    /// </summary>
    /// <returns></returns>
    private IEnumerator DissolveCoroutine()
    {
        dissolveCounter = dissolveTime;

        while (dissolveCounter > 0)
        {
            if (renderers.Length > 0)
            {
                foreach(Renderer renderer in renderers)
                {
                    renderer.GetPropertyBlock(dissolvePropertyBlock);
                    dissolvePropertyBlock.SetFloat("_CutoffHeight",
                        Mathf.Lerp(dissolveMaxHeight, dissolveMinHeight, 1 - dissolveCounter / dissolveTime));
                    renderer?.SetPropertyBlock(dissolvePropertyBlock);
                }
            }
            dissolveCounter -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //Una vez finalizada la coroutina entonces llamar al evento
        OnDissolve?.Invoke();
    }

    /// <summary>
    /// Updates the HealthBar
    /// </summary>
    private void RenderHealthBar()
    {
        if (healthBar == null) return;
        if (healthBarCoroutine != null) StopCoroutine(healthBarCoroutine);
        healthBarCoroutine = StartCoroutine(ShowHealthBar());
        healthBar.SetValue(currentHealth);
    }

    /// <summary>
    /// Muestra la barra de vide momentaneamente
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowHealthBar()
    {
        healthBar.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        healthBar.gameObject.SetActive(false);
    }
    #endregion


}
