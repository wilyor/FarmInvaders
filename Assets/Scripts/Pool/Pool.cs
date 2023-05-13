using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    #region VARIABLES
    public string id;
    public PoolEntity prefab;
    public Transform container;
    public bool isExtensible = false;
    public int prewarm = 10;
    public Queue<PoolEntity> pool = new Queue<PoolEntity>();
    public int currentAmount = 0;
    #endregion

}
