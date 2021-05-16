using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Detonates on exit of zone
/// </summary>
public class DelayedMineTrap : TrapBaseClass
{
    public SFXScriptable explodeNoise;
    GameObject m_target;
    private void Start()
    {
        TrapSetUp();
    }
    void Update()
    {
        BaseLoop();
        if(FireTrap)
        {
            Attack();
        }
        if(SwitchForm.IsInGhostForm())
        {
            CurrentCoolDown = MaxCoolDown;
        }
    }
    void Attack()
    {
        if (m_target != null)
        {
            Vector2 dir = transform.position - m_target.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -dir);
            if (hit.collider != null)
            {
                if (hit.collider.name.Contains("FloorCheck"))//Player
                {
                    m_target.GetComponent<RecieveDamage>().TakeDamage(TrapDamage, transform.position, KnockbackAmount);
                    FireTrap = false;
                }
            }
        }
        if (!FireTrap)
        {
            AudioManager.Instance.PlaySFX(explodeNoise.audioClips[0], explodeNoise.audioCaption);
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            m_target = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            m_target = null;
            DeployTrap = false;
        }
    }
}
