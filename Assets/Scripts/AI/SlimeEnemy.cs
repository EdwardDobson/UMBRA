using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : BaseEnemy
{
    List<GameObject> floorCollisions = new List<GameObject>();
    protected override void Update()
    {
        base.Update();

        if (enemyAI.currentPos == enemyAI.previousPos)        
        {
            animator.SetBool("moving", false);
        } else
        {
            animator.SetBool("moving", true);
        }
    }

    override protected void AttackPlayer()
    {
        return;
    }

    override protected void AttackAI()
    {
        return;
    }
}
