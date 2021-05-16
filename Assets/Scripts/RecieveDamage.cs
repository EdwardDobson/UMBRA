using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveDamage : MonoBehaviour
{
    public float damageBuffer = 0;
    public Vector3 lastDamagePos;
    public float knockbackBuffer;

    public void TakeDamage(float damage, Vector3 position , float knockback)
    {
        damageBuffer += damage;
        knockbackBuffer += knockback;
        lastDamagePos = position;
    }

    /*public void TakeDamafge(float damage)
    {
        damageBuffer += damage;
        lastDamagePos = transform.position;
    }*/

}
