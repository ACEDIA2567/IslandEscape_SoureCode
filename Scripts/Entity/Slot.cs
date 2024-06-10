using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public ItemData data;
    public Image icon;
    public Image delayIcon;
    public TextMeshProUGUI itemCount;
    public int index;
    public int slotQuantity;
    public float delayTime;

    PlayerCondition condition;
    PlayerController controller;

    private void Start()
    {
        condition = GameManager.Instance.Player.condition;
        controller = GameManager.Instance.Player.controller;
    }

    public void Set()
    {
        // ������ (null)���� Ȯ��
        if (data == null)
        {
            Clear();
            return;
        }
        icon.gameObject.SetActive(true);
        delayIcon.gameObject.SetActive(true);
        icon.sprite = data.icon;
        delayIcon.sprite = icon.sprite;
        itemCount.text = slotQuantity.ToString();
    }

    public void Clear()
    {
        data = null;
        icon.gameObject.SetActive(false);
        delayIcon.gameObject.SetActive(false);
        itemCount.text = string.Empty;
        slotQuantity = 0;
    }

    // ������ ��Ÿ�� ����
    public void DelayCheck()
    {
        if (data == null)
        {
            Clear();
            return;
        }
        else if (data.type != ItemType.Consumable)
        {
            return;
        }

        if (delayTime == data.delayTime)
        {
            GameManager.Instance.Player.EndCo(TImePlus());
        }
        else
        {
            GameManager.Instance.Player.StartCo(TImePlus());
        }
    }

    // ������ ������ ���� ����
    public void Use()
    {
        if (data.type == ItemType.Consumable)
        {
            UseConsumable();
        }
        else if (data.type == ItemType.Equipment)
        {
            GameManager.Instance.Player.euipment.EquipNew(data);
        }
        else if(data.type == ItemType.Build)
        {
            GameManager.Instance.Player.creaft.GetData(data, index);
        }
    }

    // �ڷ�ƾ
    IEnumerator TImePlus()
    {
        delayIcon.fillAmount = 0;
        while (delayIcon.fillAmount < 1.0f || data == null)
        {
            delayTime += Time.deltaTime;

            if (data != null)
            {
                delayTime = Mathf.Min(delayTime, data.delayTime);
                delayIcon.fillAmount = delayTime / data.delayTime;
            }
            yield return null;
        }
    }

    // ������ ��� �� �Ҹ��� �������� ������ ���� ����
    private void UseConsumable()
    {
        if (delayIcon.fillAmount != 1) return; 

        foreach (ConsumableData consumable in data.consumableData)
        {
            switch (consumable.consumableType)
            {
                case ConsumableType.Hp:
                    condition.Heal(consumable.value);
                    break;
                case ConsumableType.Sp:
                    condition.Restore(consumable.value);
                    break;
                case ConsumableType.Ep:
                    condition.Eat(consumable.value);
                    break;
                case ConsumableType.Wp:
                    condition.Drink(consumable.value);
                    break;
                case ConsumableType.SpeedUp:
                    controller.StartCoroutine(controller.AbilityUp(consumable.value, true));
                    break;
                case ConsumableType.JumpUp:
                    controller.StartCoroutine(controller.AbilityUp(consumable.value, false));
                    break;
            }

        }
        slotQuantity--;
        if (slotQuantity <= 0)
        {
            Clear();
        }
        else
        {
            delayTime = 0;
            GameManager.Instance.Player.StartCo(TImePlus());
            Set();
        }
    }

    // ������Ʈ�� ��Ŭ�� �� Use()�޼��� ����
    public void OnPointerClick(PointerEventData eventData)
    {
        if (data == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Use();
        }
        else if(eventData.button == PointerEventData.InputButton.Middle)
        {
            UICursor uiCursor = GameManager.Instance.Player.cursor;
            uiCursor.transform.position = eventData.position;
            uiCursor.UIUpdate(data);
        }
    }

    // �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������ ���� �÷��̾����� ����
        GameManager.Instance.Player.currentData = this.data;
        GameManager.Instance.Player.dataQuantity = slotQuantity;
        GameManager.Instance.Player.dataDelayTime = delayTime;

        GameManager.Instance.Player.cursor.itemDrag.SetActive(true);
        GameManager.Instance.Player.cursor.itemDrag.GetComponent<Image>().sprite = data.icon;
        Clear();
    }

    // �巡�� ������ ��
    public void OnEndDrag(PointerEventData eventData)
    {
        if (data != null)
        {
            return;
        }

        data = GameManager.Instance.Player.currentData;
        slotQuantity = GameManager.Instance.Player.dataQuantity;
        delayTime = GameManager.Instance.Player.dataDelayTime;
        DelayCheck();
        Set();
        GameManager.Instance.Player.cursor.itemDrag.SetActive(false);
    }

    // �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        // �ش� ������ �������� ��ġ�� ���콺 ��ġ�� �����ϰ�
        GameManager.Instance.Player.cursor.itemDrag.transform.position = eventData.position;
    }

    // �̵��� ������ ����
    public void OnDrop(PointerEventData eventData)
    {
        // �÷��̾��� currentData�� �ִ��� Ȯ��
        if (GameManager.Instance.Player.currentData == null) return;

        // �������� ������ ������ Ȯ�� 
        if (GameManager.Instance.Player.currentData == this.data)
        {
            // �� ���� Ȯ��
            if (GameManager.Instance.Player.dataQuantity + this.slotQuantity < this.data.maxCount)
            {
                // ������ �߰��� �� ������ ���� �߰�
                this.slotQuantity += GameManager.Instance.Player.dataQuantity;
                GameManager.Instance.Player.ItemClear();
            }
        }
        // ����� ��ҿ� �����Ͱ� ���ٸ�
        else if (data == null)
        {
            Debug.Log(GameManager.Instance.Player.currentData.itemName);
            data = GameManager.Instance.Player.currentData;
            slotQuantity = GameManager.Instance.Player.dataQuantity;
            delayTime = GameManager.Instance.Player.dataDelayTime;
            GameManager.Instance.Player.ItemClear();
        }
        // �������� ������ �ٸ��� ��ġ ����
        else
        {
            ItemData cloneData = data;
            int cloneInt = slotQuantity;
            float cloneDelayTime = delayTime;
            data = GameManager.Instance.Player.currentData;
            slotQuantity = GameManager.Instance.Player.dataQuantity;
            delayTime = GameManager.Instance.Player.dataDelayTime;

            GameManager.Instance.Player.currentData = cloneData;
            GameManager.Instance.Player.dataQuantity = cloneInt;
            GameManager.Instance.Player.dataDelayTime = cloneDelayTime;
        }

        GameManager.Instance.Player.cursor.itemDrag.SetActive(false);
        DelayCheck();
        Set();
    }
}
