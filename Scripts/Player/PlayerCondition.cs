using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour,IDamagalbe
{
    public UICondition conditions;

    private Condition hp { get { return conditions.hp; } }
    private Condition sp { get { return conditions.sp; } }
    private Condition ep { get { return conditions.ep; } }
    private Condition wp { get { return conditions.wp; } }

    public float HealthDecay;

    private void Update()
    {
        ep.Down(ep.decayValue * Time.deltaTime);
        wp.Down(wp.decayValue * Time.deltaTime);

        if(ep.currentValue <= 0f)
        {
            hp.Down(HealthDecay * Time.deltaTime);

            if (hp.currentValue <= 0)
            {
                Die();
            }
        }
        if(wp.currentValue <= 0f)
        {
            hp.Down(HealthDecay * Time.deltaTime);

            if (hp.currentValue <= 0)
            {
                Die();
            }
        }
    }

    // ��� ���
    public bool UseSp(float value)
    {
        if (sp.currentValue > value)
        {
            sp.Down(value);
            return true;
        }
        return false;
    }

    // ü�� ȸ��
    public void Heal(float value)
    {
        hp.Up(value);
    }

    // ��� ȸ��
    public void Restore(float value)
    {
        sp.Up(value);
    }

    // ����� ä���
    public void Eat(float value)
    {
        ep.Up(value);
    }

    // �� ���ñ�
    public void Drink(float value)
    {
        wp.Up(value);
    }

    // ���� ����
    public void TakePhysicalDamage(int value)
    {

        hp.Down(value);
       
        if (hp.currentValue <= 0)
        {
            Die();
        }
    }
    // �ױ�
    void Die()
    {
        GameManager.Instance.uiManager.GameOver();
    }
}
