using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;
    public float runSpeed;
    public float runSp;
    private float addSpeed;
    public Vector3 platformDir;
    private Vector2 moveInput;
    private bool runCheck;

    [Header("Jump")]
    public float jumpPower;
    public float jumpSp;
    private float addJump;
    private int jumpCount;

    // Component
    new Rigidbody rigidbody;
    PlayerCondition condition;

    // Ladder ����
    private int ladderLayerMask;
    private Vector3 ladderRayHight;
    private Ray rays;

    [Header("Camera")]
    public Transform cameraTransform;
    public float rotSpeed;
    private Vector2 mouseDelta;
    private float cameraRotX;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
    }

    private void Start()
    {
        ladderRayHight = new Vector3(0, -0.7f, 0);
        jumpCount = 1;
        Cursor.lockState = CursorLockMode.Locked;
        ladderLayerMask = LayerMask.GetMask("Ladder");
    }

    void FixedUpdate()
    {
        Move();
    }

    // Ŀ�� ��尡 ����� ���� ���� �̵� �����ϰ� ��
    private void LateUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }
    }

    // ������ ����� �����Ƽ ��� ������ �� �� �̵� �ӵ� �� ���� �� ���
    public IEnumerator AbilityUp(float time, bool movementCheck)
    {
        if (movementCheck)
        {
            addSpeed = 5;
        }
        else
        {
            addJump = 6;
        }

        yield return new WaitForSeconds(time);

        if (movementCheck)
        {
            addSpeed = 0;
        }
        else
        {
            addJump = 0;
        }
    }

    // ��ٸ��� �ִ����� ���� �̵� �� ����
    private void Move()
    {
        Vector3 moveDir;
        if (LadderCheck())
        {
            moveDir = transform.up * moveInput.y + transform.right * moveInput.x;
        }
        else
        {
            moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        }

        if (runCheck && condition.UseSp(runSp))
        {
            moveDir *= runSpeed + addSpeed;
        }
        else
        {
            moveDir *= moveSpeed + addSpeed;
        }

        if (!LadderCheck()) 
        {
            moveDir.y = rigidbody.velocity.y;
        } 
        rigidbody.velocity = moveDir + platformDir;
    }

    private void Look()
    {
        cameraRotX += mouseDelta.y * rotSpeed;
        // ��ȭ ���� ����
        cameraRotX = Mathf.Clamp(cameraRotX, -65, 65);
        cameraTransform.localEulerAngles = new Vector3(-cameraRotX, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * rotSpeed, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (jumpCount >= 1 && condition.UseSp(jumpSp))
            {
                rigidbody.AddForce(Vector2.up * (jumpPower + addJump), ForceMode.Impulse);
                jumpCount -= 1;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // ��ư�� ������ ���̶��
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        // ��ư�� �ȴ����� ��
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            runCheck = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            runCheck = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumpCount = 1;
        }
    }

    bool LadderCheck()
    {
        rays = new Ray(transform.position + ladderRayHight, transform.forward);
        if (Physics.Raycast(rays, 0.6f, ladderLayerMask))
        {
            return true;
        }
        return false;
    }
}
