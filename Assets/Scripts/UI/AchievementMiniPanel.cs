using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMiniPanel : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image star;
    public Sprite starInIcon;
    public Sprite starOffIcon;

    /// <summary>
    /// Pone los elementos dentro de la interfaz de logro
    /// </summary>
    /// <param name="achievement"></param>
    public void SetAchievement(Achievement achievement)
    {
        title.text = achievement.name;
        description.text = achievement.description;
        star.sprite = achievement.unlocked ? starInIcon : starOffIcon;
    }

}
