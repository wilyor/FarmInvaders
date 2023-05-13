using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public Image iconUI;
    public TMP_Text nameUI;
    public Animator anim;

    private void OnEnable()
    {
        AchievementManager.OnAchievementUnlocked += SetAndShow;
    }

    private void OnDestroy()
    {
        AchievementManager.OnAchievementUnlocked -= SetAndShow;

    }

    /// <summary>
    /// setea los datos y muetra el logro
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imageName"></param>
    public void SetAndShow(string name, string imageName)
    {
        nameUI.text = name;
        iconUI.sprite = Resources.Load<Sprite>("AchievementSprites/" + imageName);
        anim.SetTrigger("Show");
    }

}
