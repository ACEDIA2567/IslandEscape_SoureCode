using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData[] itemToGive;
    public GameObject applePrefab;
    public int quantityPerHit = 1;
    public int capacy;
    public GatherType type;

    // GatherType�� �´� ���� ������Ʈ Ȯ���Ͽ� ä�� �����ϰ� ��
    public void Gather(Vector3 hitPoint, Vector3 hitNormal, GatherType type)
    {
        if (this.type != type) return;

        for(int i = 0; i < quantityPerHit; i++)
        {
            capacy -= 1;
            Instantiate(itemToGive[0].dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            if (capacy <= 0)
            {
                if (type == GatherType.Wood)
                {
                    Instantiate(itemToGive[1].dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
                }
                Destroy(gameObject);
                break;
            }
        }
    }
}
