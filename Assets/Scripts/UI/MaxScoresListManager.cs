using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaxScoresListManager : MonoBehaviour
{
    public Transform container;
    public ScorePanel miniPanelPrefab;
    public ScorePanel firstPlace;


    private void Start()
    {
        RenderList();
    }

    /// <summary>
    /// Pone la lista
    /// </summary>
    public void RenderList()
    {
        PlayerInfo[] players = OrganizeList(DataManager.instance.data.players.ToArray());
        for (int i = 1; i< players.Length; i++)
        {
            CreateMiniPanel(players[i]);
        }

        if (players.Length > 0)
        {
            SetFirstPlace(players[0]);
        }
    }
    /// <summary>
    /// Crea un nuevo minipanel
    /// </summary>
    /// <param name="playerInfo"></param>
    private void CreateMiniPanel(PlayerInfo playerInfo)
    {
        ScorePanel temp = Instantiate(miniPanelPrefab, container);
        temp.SetScoreInfo(playerInfo);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="players"></param>
    private PlayerInfo[] OrganizeList(PlayerInfo[] players)
    {
        return players.OrderByDescending(o => o.maxScore).ToArray();
    }

    /// <summary>
    /// Pone la informacion del primer lugar
    /// </summary>
    /// <param name="player"></param>
    private void SetFirstPlace(PlayerInfo player)
    {
        firstPlace.SetScoreInfo(player);
    }
}
