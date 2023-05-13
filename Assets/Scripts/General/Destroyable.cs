using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destroyable : MonoBehaviour
{
    public UnityEvent OnDestroy;
    public Transform body;
    public static Action OnDestroyObject;
    /// <summary>
    /// Actions When destroyed
    /// </summary>
    public void Destroy()
    {
        body.transform.localScale = Vector3.zero;
        OnDestroy?.Invoke();
        OnDestroyObject?.Invoke();
    }
}
