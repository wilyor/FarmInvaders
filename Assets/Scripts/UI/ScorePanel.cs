using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI score;

    /// <summary>
    /// Pone los elementos dentro de la interfaz de PlayerInfo
    /// </summary>
    /// <param name="playerI"></param>
    public void SetScoreInfo(PlayerInfo playerI)
    {
        playerName.text = playerI.playerName;
        score.text = playerI.maxScore.ToString();
    }
}
