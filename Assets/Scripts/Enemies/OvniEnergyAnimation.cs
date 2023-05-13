using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvniEnergyAnimation : MonoBehaviour
{

    public Renderer rend;
    public float speed = 1;

    void Update()
    {
        AnimateEnergy();
    }

    /// <summary>
    /// Animacion mediante offset de la energia
    /// </summary>
    private void AnimateEnergy()
    {
        Vector2 newOffset = rend.material.mainTextureOffset;
        newOffset.y += Time.deltaTime * speed;
        rend.material.mainTextureOffset = newOffset;
    }
}
