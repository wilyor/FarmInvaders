using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementListManager : MonoBehaviour
{
    public Transform container;
    public AchievementMiniPanel miniPanelPrefab;

    private void Start()
    {
        Achievement[] achievements = DataManager.instance.data.achievements;
        foreach(Achievement ach in achievements)
        {
            CreateMiniPanel(ach);
        }
    }

    /// <summary>
    /// Crea un nuevo minipanel
    /// </summary>
    /// <param name="achievement"></param>
    private void CreateMiniPanel(Achievement achievement)
    {
        AchievementMiniPanel temp= Instantiate(miniPanelPrefab, container);
        temp.SetAchievement(achievement);
    }
}
