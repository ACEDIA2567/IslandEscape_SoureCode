using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCraft : MonoBehaviour
{
    ItemData data;
    GameObject PreviwerObject;

    RaycastHit hitInfo;
    public LayerMask layerMask;
    public int slotIndex;
    bool creaftMode = false;
    new Camera camera;

    private void Start()
    {
        data = null;
        camera = Camera.main;
    }

    // ������ ��� �� ���� ������ �Ͻ� ������ ������ Ȯ��
    public void GetData(ItemData data, int index)
    {
        // ũ����Ʈ ����� ��� ������ ���� ���ϰ� ��
        if (this.data != null) return;
        PreviwerObject = Instantiate(data.ViewObject);
        slotIndex = index;
        this.data = data;
    }

    private void Update()
    {
        if (data != null)
        {
            ViewerItemUpdate();
        }
    }

    // ������ ��ġ
    private void ViewerItemUpdate()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, 10, layerMask))
        {
            if (hitInfo.transform != null)
            {
                PreviwerObject.transform.position = hitInfo.point;
                if (creaftMode && PreviwerObject.GetComponent<MeshRenderer>().material.color == Color.green)
                {
                    if (data.itemName == "SOS��")
                    {
                        GameManager.Instance.stoneSOS = true;
                        GameManager.Instance.EndingCheck();
                    }
                    else if (data.itemName == "��ں�")
                    {
                        GameManager.Instance.fireCheck = true;
                        GameManager.Instance.EndingCheck();
                    }

                    Instantiate(data.dropPrefab, hitInfo.point, Quaternion.identity);
                    data = null;
                    Destroy(PreviwerObject);
                    // �κ��丮�� ������ ����
                    GameManager.Instance.Player.inventory.GetSlot(slotIndex).Clear();
                    slotIndex = -1;
                }
            }
        }
        // ESC ������ ���� ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            data = null;
            Destroy(PreviwerObject);
            slotIndex = -1;
        }
    }

    // ��Ŭ������ ������Ʈ ��ġ
    public void OnBuild(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            creaftMode = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            creaftMode = false;
        }
    }
}
