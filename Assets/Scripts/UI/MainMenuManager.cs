using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// Muestra un submenu
    /// </summary>
    /// <param name="submenu"></param>
    public void ShowSubMenu(CanvasGroup submenu)
    {
        submenu.alpha = 1;
        submenu.blocksRaycasts = true;
        submenu.interactable = true;
    }

    /// <summary>
    /// Oculta un submenu
    /// </summary>
    /// <param name="submenu"></param>
    public void HideSubMenu(CanvasGroup submenu)
    {
        submenu.alpha = 0;
        submenu.blocksRaycasts = false;
        submenu.interactable = false;
    }

    /// <summary>
    /// Para cerrar la app
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

}
