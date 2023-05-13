using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAim : MonoBehaviour
{
    public Transform aim;
    public Camera mainCamera;
    public LayerMask layer;
    // Update is called once per frame
    void Update()
    {
        SetAimPosition();
    }

    #region METHODS
    /// <summary>
    /// pone el apuntador en el suelo de acuerdo a la posición del mouse
    /// </summary>
    public void SetAimPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layer))
        {
            aim.position = raycastHit.point;
        }
    }

    #endregion
}
