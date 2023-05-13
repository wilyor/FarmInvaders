using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AchievementManager : MonoBehaviour
{
    //accion para mostrar el logro desbloqueado con nombre e imagen
    public static Action<string, string> OnAchievementUnlocked;

    private void OnEnable()
    {
        //suscribir un metodo sin nombre
        HealthCollectable.OnCollected += () => IncreaseStatAndCheckAchievement("Food", 1);
        EnemyHealth.OnDead += () => IncreaseStatAndCheckAchievement("Kill", 1);
        Destroyable.OnDestroyObject += () => IncreaseStatAndCheckAchievement("Destroyable", 1);
        EvolvingAmmunition.OnRecollected += (string code) => IncreaseStatAndCheckAchievement(code, 1);
        PlayerShooting.OnSpecialPerformed += () => IncreaseStatAndCheckAchievement("Especialist", 1);
        GameManager.OnThousand += () => IncreaseStatAndCheckAchievement("Score", 1000);
    }
    /// <summary>
    /// Incrementa una estadistica dada y verifica los logros asociados
    /// </summary>
    /// <param name="code"></param>
    /// <param name="amount"></param>
    public void IncreaseStatAndCheckAchievement(string code, int amount)
    {
        Stat stat = DataManager.instance.data.stadistics.Where(s => s.code == code).FirstOrDefault();

        if (stat == null) return;
        stat.value += amount;

        foreach (Achievement achievement in DataManager.instance.data.achievements.Where(s =>s.statCode == code 
        && s.unlocked == false).AsEnumerable())
        {
            if(stat.value >= achievement.targetAmount)
            {
                achievement.unlocked = true;
                OnAchievementUnlocked?.Invoke(achievement.name, achievement.imageName);
                DataManager.instance.SaveData();
            }
        }
    }
}
