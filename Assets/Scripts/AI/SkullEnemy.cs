using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullEnemy : BaseEnemy
{
    public GameObject projectilePrefab;

    override protected void Start()
    {
        base.Start();
    }

    override protected void AttackPlayer()
    {
        if (closestEnemy != null)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                animator.SetBool("doAttack", false);
            }
            else
            {
                Vector2 _dir = closestEnemy.transform.position - transform.position;
                Fire((_dir).normalized);
                timer = attackTimer;
                animator.SetBool("doAttack", true);
            }
        }
    }

    override protected void AttackAI()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            animator.SetBool("doAttack", false);
        }
        else
        {
            if (Vector3.Distance(transform.position, playerObject.transform.position) <= targetRange)
            {
                Fire((playerObject.transform.position - transform.position).normalized);
                timer = attackTimer;
                animator.SetBool("doAttack", true);
            }
        }
    }
    private void Fire(Vector3 targetDir)
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.transform.LookAt(targetDir);
        proj.GetComponent<Projectile>().SetVariables(projectileSpeed, new Vector2(targetDir.x, targetDir.y), canHit, IsPossessed, attackDamage);
        proj.transform.parent = transform;
    }
}
