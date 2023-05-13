using UnityEngine;

//interfaz generica, T es un placeholder de tipo gen�rico, deber� ser proporcionado cuando se implemente
public interface IDamageable<T>
{
    /// <summary>
    /// Devolver� si la clase que lo implemnta est� muerto
    /// </summary>
    /// <returns></returns>
    bool IsDead();

    //La clase que implemente esta interfaz estara obligda a definir este m�todo, default es Vector (0,0,0)
    /// <summary>
    /// Aplica el da�o, indicando la posici�n en la qeu esta se produce
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="impact"></param>
    void TakeDamage(T damage, Vector3 impact = default(Vector3));
}
