using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    private object curEquipIndex;


    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    // ��� ����
    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.dropPrefab, equipParent).GetComponent<Equip>();
    }

    // ��� ����
    public void UnEquip()
    {
        if(curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    // ��� ���� �� ��Ŭ�� �� �ൿ
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && curEquip != null)
        {
            curEquip.OnAttackInput();
        }
    }
}
