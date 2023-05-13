using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolsManager : MonoBehaviour
{
    public static PoolsManager instance;
    public Pool[] pools;

    #region EVENTS
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        InitializePool();
    }

    private void OnEnable()
    {
        //nos inscribimos al evento onReturn pool que es statico en la clase pool entity
        PoolEntity.OnReturnPool += PushElement;
    }

    private void OnDisable()
    {
        //nos desuscribimos al evento, para no dejar metodos  en el action que no puedan ser ejecutados
        PoolEntity.OnReturnPool -= PushElement;
    }
    #endregion

    #region METHODS

    /// <summary>
    /// Inicializa todas las pools creadas en el array
    /// </summary>
    private void InitializePool()
    {
        foreach(Pool pool in pools)
        {
            if (!pool.container) pool.container = transform;
            CreateSingleObject(pool);
        }
    }

    /// <summary>
    /// Instancia un elemento del pool
    /// </summary>
    /// <param name="pool"></param>
    private void CreateSingleObject(Pool pool)
    {
        PoolEntity temp;
        for (int i = 0; i < pool.prewarm; i++)
        {
            temp = Instantiate(pool.prefab, pool.container.position, pool.container.rotation, pool.container);
            temp.poolID = pool.id;
            pool.pool.Enqueue(temp);
            temp.Deactivate();
        }
        pool.currentAmount = pool.pool.Count;
    }

    /// <summary>
    ///Extrae un entity de la pool indicada, lo posiciona, rota y activa con parámetros indicados
    /// </summary>
    /// <param name="poolId"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public PoolEntity PullElement(string poolId, Vector3 position, Quaternion rotation)
    {
        PoolEntity entity = null;
        Pool pool = pools.Where(a => a.id == poolId).First();
        entity = GetPooledObject(pool);
        if(entity != null)
        {
            entity.transform.position = position;
            entity.transform.rotation = rotation;
            entity.Initialize();
        }
        return entity;
    }

    /// <summary>
    /// Vuelve a meter un entity en su correspondiente pool
    /// </summary>
    /// <param name="entity"></param>
    public void PushElement(PoolEntity entity)
    {
        Pool pool = pools.Where(a => a.id == entity.poolID).First();
        pool.pool.Enqueue(entity);
        entity.Deactivate();
        pool.currentAmount = pool.pool.Count;
    }

    /// <summary>
    /// Entrega un objeto del pool (poniendolo activo)
    /// </summary>
    /// <returns></returns>
    public PoolEntity GetPooledObject(Pool pool)
    {
        PoolEntity entity = null;
        pool.pool.TryDequeue(out entity);
        if (pool.isExtensible && entity == null)
        {
            PoolEntity temp = CreatePoolEntity(pool);
            temp.Deactivate();
            pool.pool.Enqueue(temp);
        }

        pool.currentAmount = pool.pool.Count;

        return entity;
    }

    /// <summary>
    /// Crear un nuevo entity en el pool
    /// </summary>
    /// <param name="poolId"></param>
    /// <returns></returns>
    private PoolEntity CreatePoolEntity(Pool pool)
    {
        PoolEntity entity = null;
        entity = Instantiate(pool.prefab, pool.container);
        entity.poolID = pool.id;
        return entity;
    }
    #endregion

    /// <summary>
    /// Crear un buevito de prueba
    /// </summary>
    [ContextMenu("crear huevito")]
    public void instantiateEgg()
    {
        Vector3 newPos = transform.position + new Vector3(Random.Range(-20, 20),0, Random.Range(-20, 20));
        instance.PullElement("Egg", newPos, Quaternion.identity);
    }

}