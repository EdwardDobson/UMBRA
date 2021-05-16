using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTrap : TrapBaseClass
{
    GameObject m_target;
    [SerializeField]
    GameObject m_targetEnemy;
    public float EffectDamage;
    public float ProcAmount;
    public float ProcDuration;
    private void Start()
    {
        TrapSetUp();
    }
    void Update()
    {
        BaseLoop();
        if (FireTrap)
        {
            Attack();
        }
    }
    void Attack()
    {
        if (m_target != null)
        {
            if (m_target.name == "Player")
            {
                if (!SwitchForm.IsInGhostForm())
                {
                    m_target.GetComponent<RecieveDamage>().TakeDamage(TrapDamage, transform.position, KnockbackAmount);
                    if (HasEffect)
                    {
                        BuffManager.AddEffect(ProcDuration, EffectDamage, ProcAmount, false, EffectType.Debuff, EffectAttribute.Damage);
                        BuffManager.AddEffect(1, 1, 2, true, EffectType.Debuff, EffectAttribute.Speed);
                    }
                    FireTrap = false;
                }
                else
                {
                    CurrentCoolDown = 0;
                    FireTrap = false;
                }
            }
        }
        if (m_targetEnemy != null)
        {
            m_targetEnemy.GetComponent<RecieveDamage>().TakeDamage(TrapDamage, transform.position, KnockbackAmount);
            FireTrap = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            m_target = collision.gameObject;
            DeployTrap = true;
        }
        if (collision.gameObject.tag.Contains("Enemy"))
        {
            if (!collision.gameObject.GetComponent<BaseEnemy>().isRanged)
            {
                m_targetEnemy = collision.gameObject;
                DeployTrap = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            m_target = null;
        }
        if (collision.gameObject.tag.Contains("Enemy"))
        {
            m_targetEnemy = null;
        }
    }
}
