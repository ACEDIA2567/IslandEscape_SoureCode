using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour, IItemInfo
{
    public ItemData data;

    // ������ ȹ��
    public void ItemAdd()
    {
        GameManager.Instance.Player.currentData = data;
        GameManager.Instance.Player.AddItem?.Invoke();
        Destroy(gameObject);
    }

    // ������ �ؽ�Ʈ ���� ��ȯ
    public string ItemText()
    {
        string text = data.itemName + "\n" + data.description;
        return text;
    }
}
