using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GatherType
{
    Wood,
    Stone
}

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    [Header("Resource Gathering")]
    public bool doesGatherResources;
    public GatherType type;

    [Header("cambat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera camera;

    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }
    
    void OnCanAttack()
    {
        attacking = false;
    }

    // 장비 애니메이션에 추가한
    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal, type);
            }

            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagalbe damagable))
            {
                damagable.TakePhysicalDamage(damage);
            }
        }
    }
}
