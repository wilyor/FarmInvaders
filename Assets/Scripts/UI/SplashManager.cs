using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    public float splashduration = 5;
    private float currentTime = 0;
    private bool toNextScene = false;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= splashduration && !toNextScene)
        {
            toNextScene = true;
            GoToMainMenu();
        }
    }

    /// <summary>
    /// Para ir a la pantalla de carga 
    /// </summary>
    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
