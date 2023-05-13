using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPagesManager : MonoBehaviour
{
    #region VARIABLES

    public CanvasGroup[] panels;
    private int currentPage = 0;
    #endregion

    #region METHODS
    /// <summary>
    /// Para cambiar de pagina
    /// </summary>
    /// <param name="direction"></param>
    public void MovePage(int direction)
    {
        currentPage += direction;
        if(currentPage > panels.Length - 1)
        {
            currentPage = 0;
        }
        else if(currentPage < 0)
        {
            currentPage = panels.Length - 1;
        }
        HidePages();
        ShowPage(currentPage);
    }

    /// <summary>
    /// Para ocultar las paginas
    /// </summary>
    private void HidePages()
    {
        foreach (CanvasGroup panel in panels)
        {
            panel.alpha = 0;
            panel.blocksRaycasts = false;
            panel.interactable = false;
        }
    }

    /// <summary>
    /// Muestra una pagina
    /// </summary>
    private void ShowPage(int current)
    {
        CanvasGroup panel = panels[current];
        panel.alpha = 1;
        panel.blocksRaycasts = true;
        panel.interactable = true;
    }
    #endregion

}
