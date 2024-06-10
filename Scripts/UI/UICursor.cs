using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICursor : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public TextMeshProUGUI itemDec;

    public bool invenCursor = true;
    public bool craftCursor = true;

    public GameObject itemDrag;

    ItemData data;

    void Start()
    {
        GameManager.Instance.Player.cursor = this;
        gameObject.SetActive(false);
    }

    // Ŀ�� ��� Ȯ��
    public void CursorCheck()
    {
        if (invenCursor && craftCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    // ������ ���� UI
    public void UIUpdate(ItemData data)
    {
        if (this.data != data || this.data == null)
        {
            this.data = data;
            gameObject.SetActive(true);
            itemName.text = data.itemName;
            itemType.text = data.type.ToString();
            itemDec.text = data.description;
        }
        else
        {
            this.data = null;
            gameObject.SetActive(false);
        }
    }
}
