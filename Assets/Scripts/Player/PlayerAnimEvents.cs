using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    public bool isAttacking = false;
    public bool attackMove = false;
    public bool isDead = false;

    public void AttackStartEvent()
    {
        isAttacking = true;
        attackMove = true;
    }

    public void AttackEndEvent()
    {
        isAttacking = false;
    }

    public void AttackMoveEndEvent()
    {
        attackMove = false;
    }

    public void Die()
    {
        isDead = true;
    }

    public void Live()
    {
        isDead = false;
    }

    public void PostGhostReset()
    {
        AttackEndEvent();
        AttackMoveEndEvent();
    }
}
