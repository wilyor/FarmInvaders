using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletsUIManager : MonoBehaviour
{
    public Image eggActive;
    public Image pollitoActive;
    public Image gallinaActive;
    public Image regularBulletActive;
    public TextMeshProUGUI currentBullletText;

    /// <summary>
    /// Activa la luz de acuerdo a la municion seleccionada y muestra la cantidad de balas
    /// </summary>
    /// <param name="bType"></param>
    /// <param name="amount"></param>
    public void SetActiveBullet(BulletType bType, int amount)
    {
        DeactivateBulletsHover();
        RenderAmountText(amount);
        switch (bType)
        {
            case BulletType.RegularBullet:
                regularBulletActive.enabled = true;
                currentBullletText.enabled = false;
                break;
            case BulletType.ChickBullet:
                pollitoActive.enabled = true;
                break;
            case BulletType.ChickenBullet:
                gallinaActive.enabled = true;
                break;
            case BulletType.EggBullet:
                eggActive.enabled = true;
                break;
        }
    }

    /// <summary>
    /// desactiva el brillo de todos las balas en la UI
    /// </summary>
    private void DeactivateBulletsHover()
    {
        eggActive.enabled = false;
        pollitoActive.enabled = false;
        gallinaActive.enabled = false;
        regularBulletActive.enabled = false;
    }

    /// <summary>
    /// PAra renderizar la cantidad de balas
    /// </summary>
    private void RenderAmountText(int amount)
    {
        currentBullletText.enabled = true;
        currentBullletText.text = amount.ToString();
    }
}
