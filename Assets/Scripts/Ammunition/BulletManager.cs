using System;
using System.Collections.Generic;

public enum BulletType
{
    RegularBullet,
    ChickenBullet,
    ChickBullet,
    EggBullet,
    EnemyBullet
}

[System.Serializable]
public class BulletManager
{
    public int maxAmount;
    public Dictionary<BulletType, BulletContainer> bulletContainers;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="max"></param>
    public BulletManager(int max)
    {
        maxAmount = max;
    }

    /// <summary>
    /// Inicializa el diccionario de las balas, crendo un espacio para cada tipo de bala (excepto la del enemigo)
    /// </summary>
    public void InitializeBulletDictionary()
    {
        bulletContainers = new Dictionary<BulletType, BulletContainer>();
        bulletContainers.Add(BulletType.RegularBullet, new BulletContainer(10, 0));
        bulletContainers.Add(BulletType.ChickBullet, new BulletContainer(10, 0));
        bulletContainers.Add(BulletType.ChickenBullet, new BulletContainer(10, 0));
        bulletContainers.Add(BulletType.EggBullet, new BulletContainer(10, 0));
    }

    /// <summary>
    /// Moficia la cantidad de balas de acuerdo al tipo de bala y la cantidad (puede ser negativa)
    /// </summary>
    /// <param name="bType"></param>
    /// <param name="amount"></param>
    public void ModifyBulletsAmount( BulletType bType ,int amount)
    {
        bulletContainers[bType].currenBulletAmount = Math.Clamp(bulletContainers[bType].currenBulletAmount + amount, 0, bulletContainers[bType].maxBulletAmount);
    }

    /// <summary>
    /// Retirna la cantidad actual de balas dependiendo del tipo
    /// </summary>
    /// <param name="bType"></param>
    /// <returns></returns>
    public int GetAmountByType(BulletType bType)
    {
        return bulletContainers[bType].currenBulletAmount;
    }

}

/// <summary>
/// Clase para manejar los contenedores de las balas
/// </summary>
[System.Serializable]
public class BulletContainer
{
    public int maxBulletAmount;
    public int currenBulletAmount;

    public BulletContainer(int bMax, int bCurrent)
    {
        maxBulletAmount = bMax;
        currenBulletAmount = bCurrent;
    }
}
