using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class ResourceData
{
    public string ResourceName;
    public int value;
}

public class Produce : MonoBehaviour
{
    // ���� �� ���� ������ ������
    public ItemData data;
    public List<ResourceData> resourceData;
    
    // �κ��丮 ����
    UIInventory inventory;

    // ���� �������� Ȯ��
    public void CheckProduce()
    {
        inventory = GameManager.Instance.Player.inventory;
        bool Check = true;

        for (int i = 0; i < resourceData.Count; i++)
        {
            // ������ ���� �ʴٸ�
            if (!inventory.CheckItem(resourceData[i].ResourceName, resourceData[i].value))
            {
                // ���� �Ұ���
                Check = false;
            }
        }

        // �������� ���簡 ������ ���� ���
        if (!Check) return;

        // ���ۿ� �ʿ��� �������� �Ҹ�
        for (int i = 0; i < resourceData.Count; i++)
        {
            // ������ �Ҹ��ϰ� ������ ȹ��
            inventory.RemoveItem(resourceData[i].ResourceName, resourceData[i].value);
        }

        // ������ ȹ��
        ProduceItem();
    }



    // ���� ���� �� �κ��丮�� ������ �ֱ�
    public void ProduceItem()
    {
        // �������� �÷��̾��� �κ��丮�� �߰�
        GameManager.Instance.Player.currentData = data;
        GameManager.Instance.Player.AddItem?.Invoke();
    }
}
