using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    #region VARIABLES
    public Animator animator;
    [Header("Shooting")]
    public float shootDelay = 0.5f;
    private float shootTime = 0;
    public Transform cannon;
    //id de la pool que quiero ercuperar los proyectiles
    public List <BulletType> bulletType ;
    public int currentBullet = 0;
    public Transform aim;
    public BulletManager bulletManager;
    public int maxBullets= 10;


    [Header("Special")]
    public SpecialBar specialBar;
    public float maxValueToSpecial = 50f;
    public float currentValueToSpecial = 0f;

    public UnityEvent OnSpecialLaunch;
    public static Action OnSpecialPerformed;
    #endregion

    #region EVENTS
    private void Start()
    {
        bulletManager = new BulletManager(maxBullets);
        bulletManager.InitializeBulletDictionary();
        ChangeBulletType(0);
        RefillBullets();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Dispara el cañon
    /// </summary>
    public void Shoot()
    {
        if (Time.time < shootTime) return;
        animator.SetTrigger("Shoot");
        ConfirmShoot();
        animator.SetFloat("ShootSpeed", 1 / shootDelay);
        shootTime = Time.time + shootDelay;
    }

    /// <summary>
    /// Revisa si hay balas disponibles para poder disparar
    /// </summary>
    public void ConfirmShoot()
    {
        if(bulletManager.GetAmountByType(bulletType[currentBullet]) > 0 || bulletType[currentBullet] == BulletType.RegularBullet)
        {
            var temp = PoolsManager.instance.PullElement(bulletType[currentBullet].ToString(), cannon.position, Quaternion.LookRotation(cannon.forward)) as Projectile;
            if (temp != null)
            {
                temp.shooterPosition = transform;
                AddBullets(bulletType[currentBullet], -1);
                GUIManager.instance.RenderBullets(bulletType[currentBullet], bulletManager.GetAmountByType(bulletType[currentBullet]));
            }
        }
    }

    /// <summary>
    /// Cambia el indice de la bala actual
    /// </summary>
    public void ChangeBulletType(int value = 1)
    {
        if(value == 0) currentBullet = 0;
        else
        {
            currentBullet+=value;
            if (currentBullet > bulletType.Count - 1)
            {
                currentBullet = 0;
            }
            if (currentBullet < 0)
            {
                currentBullet = bulletType.Count - 1;
            }
        }
        GUIManager.instance.RenderBullets(bulletType[currentBullet], bulletManager.GetAmountByType(bulletType[currentBullet]));
        aim.gameObject.SetActive(bulletType[currentBullet] != BulletType.RegularBullet );
    }

    /// <summary>
    /// Añade balas a los cartuchos
    /// </summary>
    public void AddBullets(BulletType bulletType, int amount)
    {
        bulletManager.ModifyBulletsAmount(bulletType, amount);
    }

    /// <summary>
    /// para testing, rellena todas las balas
    /// </summary>
    [ContextMenu ("Refill")]
    public void RefillBullets()
    {
        AddBullets(BulletType.ChickenBullet, 5);
        AddBullets(BulletType.ChickBullet, 5);
        AddBullets(BulletType.EggBullet, 5);
    }

    /// <summary>
    /// Acciones para Lanzar poder Especial
    /// </summary>
    public void ShootSpecial()
    {
        if(currentValueToSpecial >= maxValueToSpecial)
        {
            currentValueToSpecial = 0;
            specialBar.InitializeBar(maxValueToSpecial);
            OnSpecialLaunch.Invoke();
            OnSpecialPerformed?.Invoke();
        }
    }

    /// <summary>
    /// refills the special Bar
    /// </summary>
    [ContextMenu("Reload Special")]
    public void FillAll()
    {
        AddPowerToSpecial(maxValueToSpecial);
    }

    /// <summary>
    /// Añade energia al especial
    /// </summary>
    /// <param name="value"></param>
    public void AddPowerToSpecial(float value)
    {
        currentValueToSpecial += value;
        specialBar.SetValue(currentValueToSpecial);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            ICollectable colectable;
            if (other.TryGetComponent<ICollectable>(out colectable))
            {
                colectable.OnCollect(this);
            }
        }
    }
    #endregion
}
