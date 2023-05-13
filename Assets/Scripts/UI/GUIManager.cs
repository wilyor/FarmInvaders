using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public CanvasGroup PauseMenu;
    public CanvasGroup gameOverMenu;
    public CanvasGroup newRecord;
    public CanvasGroup cheats;
    public Animator flowersAnim;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI HUDScore;

    public TMP_InputField userName;

    public BulletsUIManager bulletsUIManager;
    public static GUIManager instance;
    public bool isGameOver = false;

    #region EVENTS

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(Time.timeScale == 0 ? 1 : 0);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            cheats.alpha = cheats.alpha == 0 ? 1 : 0;
            cheats.blocksRaycasts = !cheats.blocksRaycasts;
            cheats.interactable = !cheats.interactable;

        }
    }
    #endregion

    #region METHODS
    /// <summary>
    /// TParapausar/despausar el juego
    /// </summary>
    public void PauseGame(int timeScale)
    {
        if(!isGameOver)
        Time.timeScale = timeScale;
        PauseMenu.alpha = timeScale == 0 ? 1 : 0;
        PauseMenu.blocksRaycasts = timeScale == 0;
        PauseMenu.interactable = timeScale == 0;
    }

    /// <summary>
    /// Para ir a otra escena
    /// </summary>
    public void GoToScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Para reiniciar Nivel
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    /// <summary>
    /// Para renderizar la municion
    /// </summary>
    /// <param name="bType"></param>
    /// <param name="bulletAmount"></param>
    public void RenderBullets (BulletType bType, int bulletAmount){
        bulletsUIManager.SetActiveBullet(bType, bulletAmount);
    }

    /// <summary>
    /// Muestra pantalla de GameOver
    /// </summary>
    public void ShowGameOver()
    {
        gameOverMenu.alpha = 1;
        gameOverMenu.blocksRaycasts = true;
        gameOverMenu.interactable = true;
        int currentScore = GameManager.instance.GetcurrentScore();
        PlayerInfo maxPlayer = DataManager.instance.data.players.Where(s => s.maxScore < currentScore).FirstOrDefault();

        currentScoreText.text = currentScore.ToString();
        ShowNewRecord(maxPlayer != null);
    }

    /// <summary>
    /// Muestra contenido extra si es un nuevo record
    /// </summary>
    /// <param name="isNewRecord"></param>
    private void ShowNewRecord(bool isNewRecord)
    {
        if (isNewRecord)
        {
            newRecord.alpha = 1;
            newRecord.blocksRaycasts = true;
            newRecord.interactable = true;
            flowersAnim.SetTrigger("grow");
        }
    }

    /// <summary>
    /// Guarda la puntuacion
    /// </summary>
    public void SaveScore()
    {
        string user = userName.text;
        int currentScore = GameManager.instance.GetcurrentScore();
        Debug.Log(user + "--" + currentScore);
        DataManager.instance.SavePlayer(user, currentScore);
    }

    /// <summary>
    /// Para actualizar la puntuacion en el HUD
    /// </summary>
    /// <param name="score"></param>
    public void RenderScore(int score)
    {
        HUDScore.text = score.ToString();
    }
    #endregion


}
