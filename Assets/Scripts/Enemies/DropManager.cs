using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public string [] dropable ;
    public float probability = 60;

    /// <summary>
    /// Suelta un elemento
    /// </summary>
    public void Drop()
    {
        if (Random.Range(0, 100) < probability) return;
        PoolsManager.instance.PullElement(SelectDrop(), transform.position, transform.rotation);
    }

    /// <summary>
    /// selecciona que elemento dropear
    /// </summary>
    /// <returns></returns>
    public string SelectDrop()
    {
        return dropable[Random.Range(0, dropable.Length)];
    }
}
