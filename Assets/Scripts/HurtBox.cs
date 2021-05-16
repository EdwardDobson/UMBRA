using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public float damageVal = 0;
    public float knockbackVal = 0;
    public GameObject self;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.Equals(self))
        {
            RecieveDamage hitDmg = collision.gameObject.GetComponent<RecieveDamage>();
            if(hitDmg != null)
            {
                Debug.Log("Hit " + collision.name);
                //hitDmg.damageBuffer += damageVal;
                hitDmg.TakeDamage(damageVal, self.transform.position, knockbackVal);
            }
        }
    }
}
