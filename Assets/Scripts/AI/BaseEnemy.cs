using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EEnemyType
{
    Grounded,
    Flying
}

public class BaseEnemy : EnemyPossession
{
    
    public float attackDamage;
    public float targetRange;
    public float attackTimer = 1f;
    public float projectileSpeed = 5;
    public float health = 20;
    public float KnockbackAmount;
    public Side canHit;
    public bool isRanged;
    protected float timer = 0f;
    protected float deathTimer = 0.5f;
    protected float dTimer = 0f;

    //Componants
    protected GameObject closestEnemy;
    protected Rigidbody2D rigidBody2D;
    protected EnemyAI enemyAI;
    protected Animator animator;
    protected RecieveDamage damage;
    protected GameObject playerObject;
    public GameObject DeathParticleObj;
    //Attack delay
    private float lastTimeAttacked = 0;
    [SerializeField]
    float attackDelay = 0.5f; //Higher means slower - e.g. 0.1 is an attack every 0.1 seconds

    public SFXScriptable damagedSFX;

    protected virtual void Start()
    {
        playerObject = PlayerController.Instance.gameObject;

        if (damage == null)
        {
            if (GetComponent<RecieveDamage>() == null)
            {
                Debug.LogError("RecieveDamage Script not found on " + gameObject.name);
                damage = gameObject.AddComponent<RecieveDamage>();
            } else
            {
                damage = GetComponent<RecieveDamage>();
            }
        }

        if (GetComponent<EnemyAI>() == null)
        {
            Debug.LogError("EnemyAI Script not found on " + gameObject.name);
            enemyAI = gameObject.AddComponent<EnemyAI>();
        } else
        {
            enemyAI = GetComponent<EnemyAI>();
        }

        GameObject sprite = transform.Find("Sprite").gameObject;
        if (sprite == null)
        {
            Debug.LogError("No sprite child object named 'Sprite' found on " + gameObject.name);
        }
        else
        {

            if (sprite.GetComponent<Animator>() == null)
            {
                Debug.LogError("No animator found on " + gameObject.name + ", " + sprite.name);
            }
            else
            {
                animator = sprite.GetComponent<Animator>();
            }
        }

        if (GetComponent<EnemyAI>() == null)
        {
            Debug.LogError("EnemyAI Script not found on " + gameObject.name);
            gameObject.AddComponent<EnemyAI>();
        }

    }

    protected virtual void Update()
    {

        health -= damage.damageBuffer;
        if (damage.damageBuffer > 0)
        {
            AudioManager.Instance.PlaySFX(damagedSFX.audioClips[0], damagedSFX.audioCaption);
            KnockBack();
            if (DeathParticleObj != null)
            {
                DeathParticleObj.GetComponent<DeathParticle>().StartColour = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                Instantiate(DeathParticleObj, transform.position, Quaternion.identity);
            }
        }

        damage.damageBuffer = 0;

        if (IsPossessed && IsPossessable)
        {
            MovementInput();
            PerformAction();
            if (GetComponent<Rigidbody2D>() == null)
            {
                m_rigidbody2D = gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
                m_rigidbody2D.gravityScale = 0;
                m_rigidbody2D.freezeRotation = true;
            }
            if (ShouldPerformAction)
            {
                closestEnemy = null;
                List<Collider2D> otherEnemies = new List<Collider2D>();
                otherEnemies = Physics2D.OverlapCircleAll(transform.position, targetRange).ToList();
                otherEnemies = otherEnemies.Where(g => g.tag.Contains("Enemy") && g.name != gameObject.name).ToList();
                otherEnemies.OrderBy(c => c.transform.position);
                if (otherEnemies.Count > 0)
                {
                    closestEnemy = otherEnemies[0].gameObject;
                }
            }
        }
        else
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                //Destroy(GetComponent<Rigidbody2D>());
            }
        }

        if (health <= 0)
        {
            //animator.SetBool("die", true);
            Destroy(gameObject);
            return;
        }
        if (!IsPossessed)
        {
            AttackAI();
        }
        if (IsPossessed)
        {
            if (ShouldPerformAction)
            {
                AttackPlayer();
            }
        }
        if (!IsPossessable)
        {
            AttackAI();
        }
    }

    private void FixedUpdate()
    {
        if (IsPossessed)
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                m_rigidbody2D = GetComponent<Rigidbody2D>();
                Movement(m_rigidbody2D, MoveSpeed);
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {            

        if (collision.gameObject.tag.Contains("Player") && !IsPossessed)
        {
            if (Time.time - lastTimeAttacked > attackDelay)
            {
                lastTimeAttacked = Time.time;
                collision.gameObject.GetComponent<RecieveDamage>().TakeDamage(attackDamage,transform.position, KnockbackAmount);
            }
        }
        if(IsPossessed && collision.gameObject.tag.Contains("Enemy"))
        {
            if(collision.gameObject != gameObject)
            {
                if (Time.time - lastTimeAttacked > attackDelay)
                {
                    lastTimeAttacked = Time.time;
                    collision.gameObject.GetComponent<RecieveDamage>().TakeDamage(attackDamage, transform.position, KnockbackAmount);
                }
            }
          
        }
    }

    private void KnockBack()
    {
        m_rigidbody2D.velocity = (transform.position - damage.lastDamagePos).normalized * 8;
    }

    virtual protected void AttackPlayer()
    {
        return;
    }
    virtual protected void AttackAI()
    {
        return;
    }

}
