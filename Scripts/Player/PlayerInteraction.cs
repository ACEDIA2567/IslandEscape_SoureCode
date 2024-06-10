using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float rayDistance;
    public LayerMask interactionLayer;
    public LayerMask tentLayer;
    public LayerMask waterLayer;
    public TextMeshProUGUI interactionText;
    public GameObject sleepUI;
    private float rateTime = 0;
    private float checkTime = 0.1f;
    private bool waterCheck = false;
    private bool sleepCheck = false;
    new private Camera camera;

    private IItemInfo itemInfo;
    private GameObject interactionObject;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - rateTime > checkTime)
        {
            rateTime = Time.time;
            Interaction();
        }
    }

    // Layer�� ���� ��ȣ�ۿ�
    private void Interaction()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, rayDistance, interactionLayer))
        {
            // �ش� ������Ʈ UI�� ������
            if (interactionObject == null)
            {
                interactionObject = hit.collider.gameObject;
                itemInfo = hit.collider.GetComponent<IItemInfo>();
                VoidText();
            }
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, tentLayer))
        {
            // �� �� ������ ���� Ȯ��
            sleepCheck = true;
            itemInfo = hit.collider.GetComponent<IItemInfo>();
            VoidText();
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, waterLayer))
        {
            if (waterCheck)
            {
                GameManager.Instance.Player.condition.Drink(10);
            }
        }
        else
        {
            sleepCheck = false;
            interactionObject = null;
            itemInfo = null;
            interactionText.gameObject.SetActive(false);
        }
    }

    // ��ȣ�ۿ� ������ ������ ���� ���
    private void VoidText()
    {
        interactionText.gameObject.SetActive(true);
        interactionText.text = itemInfo.ItemText();
    }

    // ��ȣ�ۿ� Ű �Է�
    public void GetItem(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            if(interactionObject != null)
            {
                itemInfo.ItemAdd();
                interactionObject = null;
                itemInfo = null;
                interactionText.gameObject.SetActive(false);
            }
        }
        else if (context.phase == InputActionPhase.Started)
        {
            waterCheck = true;
            if (sleepCheck)
            {
                sleepUI.SetActive(true);
                sleepCheck = false;
                itemInfo = null;
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            waterCheck = false;
        }
    }
}
