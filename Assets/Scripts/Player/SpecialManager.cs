using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialManager : MonoBehaviour
{
    public int maxAmount = 15;
    public Transform beginZone;
    public float sizeZone;
    public string MissileToDrop = "MilkPool";

    /// <summary>
    /// Lanza tinajas de leche desde el aire al suelo
    /// </summary>
    public void LaunchMilk()
    {
        for(int i = 0; i< maxAmount; i++)
        {
            Vector3 zoneToDrop = new Vector3(Random.Range(beginZone.position.x - sizeZone, beginZone.position.x + sizeZone), Random.Range(transform.position.y - 2, transform.position.y + 20), Random.Range(beginZone.position.z - sizeZone, beginZone.position.z + sizeZone));
            PoolsManager.instance.PullElement(MissileToDrop, zoneToDrop, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(beginZone.position, new Vector3 (sizeZone, 2, sizeZone));
    }
}
