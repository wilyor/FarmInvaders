using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamageable<float>
{
    #region VARIABLES
    public float maxHealth = 100;
    public float currentHealth;

    [Header("HUD")]
    public ProgressBarManager healthBar;
    public Image damageImage;

    [Header("OnDead")]
    //Acciones y efectos a realizar cuando el jugador muere
    public UnityEvent OnDead;
    //action static para informar a observadores
    public static Action OnPlayerDead;
    public bool isDead;
    public bool damaged;
    public Animator animTorre;
    public Animator animCuerpo;
    public UnityEvent OnShieldActivate;
    public bool ShieldActivate = false;


    public Color flashColor = new Color(0,0,0,0.5f);
    public Color resetColor = new Color(0, 0, 0, 0f);

    public float flashSpeed = 1.0f;
    #endregion

    #region EVENTS
    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar)
        {
            healthBar.InitializeBar(maxHealth);
        }
    }

    void Update()
    {
        FlashDamage();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Devuelve si el jugador esta muerto
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    /// <summary>
    /// acciones cuando el jugador toma daño
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="impact"></param>
    public void TakeDamage(float damage, Vector3 impact = default)
    {
        if (IsDead() || ShieldActivate) return;
        damaged = true;
        currentHealth -= damage;
        healthBar.SetValue(currentHealth);

        if (IsDead())
        {
            Die();
        }
    }

    /// <summary>
    /// Cura cierta cantidad a la salud
    /// </summary>
    /// <param name="healthValue"></param>
    public void Cure(float healthValue)
    {
        Debug.Log("Curing");
        currentHealth += healthValue;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetValue(currentHealth);
    }

    /// <summary>
    /// Acciones necesarias para la muerte del jugador
    /// </summary>
    private void Die()
    {
        animTorre.SetBool("Death", true);
        animCuerpo.SetBool("Death", true);

        OnPlayerDead?.Invoke();
        OnDead.Invoke();
    }

    /// <summary>
    /// gestiona la imagen de daño del jugador
    /// </summary>
    private void FlashDamage()
    {
        if (damaged)
        {
            damageImage.color = flashColor;
            damaged = false;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, resetColor, Time.deltaTime * flashSpeed);
        }
    }

    /// <summary>
    /// For testing
    /// </summary>
    [ContextMenu("Take Damage")]
    public void TestTakeDamage(){
        TakeDamage(50);
    }

    /// <summary>
    /// Activa el escudo
    /// </summary>
    public void ActivateShield()
    {
        if (ShieldActivate) return;
        ShieldActivate = true;
        OnShieldActivate.Invoke();
    }

    /// <summary>
    /// desactiva el escudo
    /// </summary>
    public void DeactivateShield()
    {
        ShieldActivate = false;
    }
    #endregion

}
