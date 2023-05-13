using UnityEngine;

//interfaz generica, T es un placeholder de tipo genérico, deberá ser proporcionado cuando se implemente
public interface IDamageable<T>
{
    /// <summary>
    /// Devolverá si la clase que lo implemnta está muerto
    /// </summary>
    /// <returns></returns>
    bool IsDead();

    //La clase que implemente esta interfaz estara obligda a definir este método, default es Vector (0,0,0)
    /// <summary>
    /// Aplica el daño, indicando la posición en la qeu esta se produce
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="impact"></param>
    void TakeDamage(T damage, Vector3 impact = default(Vector3));
}
