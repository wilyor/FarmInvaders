using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableGenerator : MonoBehaviour
{
    #region VARIABLES
    public Transform SpawnZoneBegin;
    public Transform SpawnZoneEnd;
    public int maxElements = 10;
    public List<PoolEntity> currentCollectables;
    public static CollectableGenerator instance;
    public int initialElements = 5;
    public float minGenerationTime = 6;
    public float maxGenerationTime = 12;

    #endregion

    #region EVENTS
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        for(int i = 0; i< initialElements; i++)
        {
            generateOvo();
        }

        StartCoroutine(GenerateElement());
    }

    /// <summary>
    /// Crea un huevo en escena
    /// </summary>
    [ContextMenu("huevo")]
    public void generateOvo()
    {
        GenerateCollectable("Egg");
    }

    private void OnEnable()
    {
        //nos inscribimos al evento onReturn pool que es statico en la clase pool entity
        PoolEntity.OnReturnPool += RemoveCollectableFromList;
    }

    private void OnDisable()
    {
        //nos desuscribimos al evento, para no dejar metodos  en el action que no puedan ser ejecutados
        PoolEntity.OnReturnPool -= RemoveCollectableFromList;
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Genera un collectable en alguna posición del mapa
    /// </summary>
    /// <param name="collectableToInstantiate"></param>
    public void GenerateCollectable(string collectableToInstantiate)
    {
        Vector3 randomPos = new Vector3(Random.Range(SpawnZoneBegin.position.x, SpawnZoneEnd.position.x), 0, Random.Range(SpawnZoneBegin.position.z, SpawnZoneEnd.position.z));
        PoolEntity currentCollectable = PoolsManager.instance.PullElement(collectableToInstantiate, randomPos, Quaternion.identity);
        currentCollectables.Add(currentCollectable);
    }


    [ContextMenu("Destroy All Collectables")]
    /// <summary>
    /// Devuelve todos los collectables al pool
    /// </summary>
    public void DestroyAllCollectables()
    {
        foreach (PoolEntity entity in currentCollectables)
        {
            entity.ReturnToPool();
        }
    }

    /// <summary>
    /// Remover un collectable de la lista
    /// </summary>
    /// <param name="entity"></param>
    public void RemoveCollectableFromList(PoolEntity entity)
    {
        currentCollectables.Remove(entity);
    }

    /// <summary>
    /// Reemplaza un elemento en la lista por otro
    /// </summary>
    public void AddEntityToList(PoolEntity newObj)
    {
        currentCollectables.Add(newObj);
    }

    /// <summary>
    /// Ienumerator para generar elementos en escena
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateElement()
    {
        yield return new WaitForSeconds(Random.Range(minGenerationTime, maxGenerationTime));
        generateOvo();
        StartCoroutine(GenerateElement());
    }
    #endregion
}
