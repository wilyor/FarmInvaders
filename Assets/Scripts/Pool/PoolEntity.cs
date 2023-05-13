using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PoolEntity : MonoBehaviour
{
    #region VARIABLES
    public string poolID;
    public static Action<PoolEntity> OnReturnPool;
    public bool active;
    public GameObject model;
    #endregion
    void Start()
    {
        
    }

    #region METHODS

    /// <summary>
    /// Acciones a realizar al sacar un objeto de la pool
    /// </summary>
    public virtual void Initialize()
    {
        active = true;
        model.transform.localScale = Vector3.one;
    }
    
    /// <summary>
    /// Acciones a realizar al devolver un objeto a la pool
    /// </summary>
    public virtual void Deactivate()
    {
        active = false;
        model.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// desactiva el objeto e informa que quiere volver a la Pool para que sea recogido por el manager
    /// </summary>
    public void ReturnToPool()
    {
        Deactivate();
        OnReturnPool?.Invoke(this);
    }
    #endregion

}
