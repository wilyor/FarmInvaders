using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    #region VARIABLES
    public float maxValue;
    public Image barImage;
    public Gradient barColor;
    public bool useGradient = false;
    public float CurrentValue { get; set; }
    #endregion

    #region METHODS

    /// <summary>
    /// Inicializa los valores de la barra
    /// </summary>
    /// <param name="maxVal"></param>
    public virtual void InitializeBar(float maxVal)
    {
        maxValue = maxVal;
        SetValue(maxVal);
    }


    /// <summary>
    /// Incrementa o disminuye el valor de la barra en el valor indicado
    /// </summary>
    /// <param name="newValue"></param>
    public void SetValue(float newValue)
    {
        CurrentValue = newValue;
        RenderBar();
    }

    /// <summary>
    /// Dibuja la barra, actualiza el valor en la imagen
    /// </summary>
    public virtual void RenderBar()
    {
        barImage.fillAmount = CurrentValue / maxValue;
        if (useGradient)
        {
            barImage.color = barColor.Evaluate(CurrentValue/maxValue);
        }
    }
    #endregion
}
