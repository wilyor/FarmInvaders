using UnityEngine;

public interface ICollectable
{
    /// <summary>
    /// Acciones cuando se recolecte
    /// </summary>
    /// <param name="shooting"></param>
    void OnCollect(PlayerShooting shooting);
}
