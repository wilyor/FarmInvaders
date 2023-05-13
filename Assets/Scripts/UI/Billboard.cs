using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este clase hace que las UI de los elementos en pantalla siempre miren a camara y se mantengan en la posición que deben
/// </summary>
public class Billboard : MonoBehaviour
{
    #region VARIABLES
    private Camera mainCamera;
    private Vector3 offset;
    private Transform parent;
    #endregion

    #region EVENTS
    void Awake()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
        parent = transform.parent;
        offset = parent.position - transform.position;
    }

    private void LateUpdate()
    {
        UpdatePositionAndRotation();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Actualiza la posición del objeto para que siempre mire a la camara (y no se mueva del lugar donde está al lado del objeto padre)
    /// </summary>
    void UpdatePositionAndRotation()
    {
        transform.position = parent.position - offset;
        transform.LookAt(mainCamera.transform);
    }
    #endregion
}
