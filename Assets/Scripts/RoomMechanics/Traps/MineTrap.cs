using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Detonates on entry
/// </summary>
public class MineTrap : TrapBaseClass
{
    GameObject m_target;
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
            Vector2 dir = transform.position - m_target.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -dir);
            Debug.DrawRay(transform.position, -dir);
            if (hit.collider != null)
            {
                if (hit.collider.name.Contains("FloorCheck"))//Player
                {
                    m_target.GetComponent<RecieveDamage>().TakeDamage(TrapDamage, transform.position, KnockbackAmount);
                    m_target.GetComponent<BuffManager>().AddEffect(2, 1, 2,true, EffectType.Debuff, EffectAttribute.Speed);
                    FireTrap = false;
                }

            }
        }
        if (!FireTrap)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            m_target = collision.gameObject;
            CurrentCoolDown = MaxCoolDown;
            DeployTrap = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            m_target = collision.gameObject;
            DeployTrap = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            m_target = null;
            DeployTrap = false;
            CurrentCoolDown = MaxCoolDown;
        }
    }
}
