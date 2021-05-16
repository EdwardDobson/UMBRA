using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    public Animator animator;
    public PlayerAnimEvents events;
    public RecieveDamage damage;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public TetherScript tether;

    public float sanity = 100;
    public float maxSanity = 100;
    public float sanityRegenAmount;
    public float sanityRegenCoolDown;
    public float sanityRegenCoolDownMax;

    public float IFramesLength;
    float currentIFramesTimer;
    public bool TakenDamage;
    public int souls;

    public bool canControl = true;
    public bool isFalling = false;
    public bool gameOver = false;


    Vector2 moveAxis;
    float moveSpeed = 3f;
    //bool attackInput = false;
    Vector2 facingAxis;
    float attackMoveDuration;
    bool attackMove = false;
    bool perfectAttack = false;
    public Vector3 previousPosition;

    Vector2 knockbackVelocity = Vector2.zero;
    float knockbackAmount;
    public SFXScriptable swordSFX;
    int count = -1;

    private void Awake()
    {

        //Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
        sanityRegenCoolDown = sanityRegenCoolDownMax;
    }

    public GameObject ghost;
    public Animator ghostAnimator;
    public Rigidbody2D grb;
    bool isGhost = false;
    bool isPossess = false;
    float respawnTimer = 0;

    List<GameObject> floorCollisions = new List<GameObject>();
    public Room currentRoom;

    ControlBindings m_cBindings;

    // Start is called before the first frame update
    void Start()
    {        
        m_cBindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
        currentIFramesTimer = IFramesLength;
        TerrainGridPoint.CreateGrid(); //Create the pathfinding grid
        AudioManager.Instance.StartMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if(!TakenDamage)
        {
            sanity -= damage.damageBuffer;
            knockbackAmount = damage.knockbackBuffer;
            if (damage.damageBuffer > 0)
            {
                KnockBack();
                TakenDamage = true;
            }
            damage.damageBuffer = 0;
            damage.knockbackBuffer = 0;
        }
        if(TakenDamage)
        {
            currentIFramesTimer -= Time.deltaTime;
            if (currentIFramesTimer <= 0)
            {
                TakenDamage = false;
                currentIFramesTimer = IFramesLength;
            }
        }
        if(sanity<= 0)
        {
            SwitchToPlayer();
            gameOver = true;
            animator.SetFloat("MoveY", 0);
            animator.SetFloat("MoveX", 0);
            animator.SetBool("DoAttack", false);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsDead", true);
            //events.isDead = true;
            //canControl = false;
            Debug.Log("dead");
            respawnTimer += Time.deltaTime;
        }
        else
        {
            respawnTimer = 0;
        }

        if(respawnTimer >= 2)
        {
            /*RoomManager.Instance.respawnPos = RoomManager.Instance.currentRoom.spawnPos;
            gameOver = false;
            sanity = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            */

            //if(currentRoom != null)
            //currentRoom.manager.Spawn(currentRoom.roomNumber);
            /*
            Transform childTransform = transform.GetChild(0);
            childTransform.localPosition = new Vector3(0,0.5f,0);
            childTransform.localScale = Vector3.one;
            transform.position = currentRoom.spawnPos;

            Vector3 cameraTarget = transform.position;

            Camera.main.transform.position = new Vector3(cameraTarget.x, cameraTarget.y, Camera.main.transform.position.z);

            sr.sortingOrder = 91;
            GetComponent<EnitySpriteSort>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            transform.GetChild(2).gameObject.SetActive(true);

            sanity = maxSanity;
            isFalling = false;
            events.isDead = false;
            animator.SetBool("IsDead", false);
            Debug.Log("undead");
            gameOver = false;*/
        }
        if (events.isDead)
        {
            moveAxis = Vector2.zero;
            animator.SetFloat("MoveY", 0);
            animator.SetFloat("MoveX", 0);
            animator.SetBool("DoAttack", false);
            animator.SetBool("IsAttacking", false);
            //animator.SetBool("IsDead", true);
            animator.SetBool("CanInput", false);
            canControl = false;
        }
        
        if (!canControl)
        {
            if (isFalling)
            {
                sanity -= Time.deltaTime * 50;
                Transform childTransform = transform.GetChild(0);
                childTransform.position = childTransform.transform.position + new Vector3(0, Time.deltaTime * -2f * childTransform.localScale.x, 0);
                childTransform.localScale = childTransform.localScale * ((1 - Time.deltaTime));
                if(childTransform.localScale.x < 0.8f) sr.sortingOrder = -1;


                moveAxis = Vector2.zero;
                animator.SetFloat("MoveY", 0);
                animator.SetFloat("MoveX", 0);
                animator.SetBool("DoAttack", false);
                animator.SetBool("IsAttacking", false);
            }

            if (!events.isDead && !isFalling)
            {
                //animator.SetBool("IsDead", false);
                animator.SetBool("CanInput", true);
                canControl = true;
            }
        }
        else
        {
            if (!attackMove && Input.GetKeyDown(m_cBindings.Interact))//Input.GetButtonDown("ButtonA"))
            {
                perfectAttack = true;                
            }

            if (perfectAttack || (!events.isAttacking && Input.GetKeyDown(m_cBindings.Interact)))//Input.GetButton("ButtonA")))
            {                
                animator.SetBool("DoAttack", true);
            }
            else
            {                
                animator.SetBool("DoAttack", false);
            }

            if (!events.isAttacking) // no input during attacks
            {
                count = -1;
                MovementInput();

                animator.SetBool("IsAttacking", false);
                if (moveAxis.x > 0)//rightwards
                {
                    animator.SetFloat("MoveX", 1);
                    animator.SetFloat("MoveY", 0);
                    animator.SetInteger("Facing", 0);
                    facingAxis = new Vector2(1, 0);
                }
                else if (moveAxis.x < 0)//leftwards
                {
                    animator.SetFloat("MoveX", -1);
                    animator.SetFloat("MoveY", 0);
                    animator.SetInteger("Facing", 1);
                    facingAxis = new Vector2(-1, 0);
                }
                else if (moveAxis.y > 0)//downwards
                {
                    animator.SetFloat("MoveX", 0);
                    animator.SetFloat("MoveY", 1);
                    animator.SetInteger("Facing", 3);
                    facingAxis = new Vector2(0, 1);
                }
                else if (moveAxis.y < 0)//upwards
                {
                    animator.SetFloat("MoveX", 0);
                    animator.SetFloat("MoveY", -1);
                    animator.SetInteger("Facing", 2);
                    facingAxis = new Vector2(0, -1);
                }
                else//still
                {
                    animator.SetFloat("MoveX", 0);
                    animator.SetFloat("MoveY", 0);
                }

            }
            else
            {
                moveAxis = Vector2.zero;
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", 0);
                //animator.SetBool("DoAttack", false);
                animator.SetBool("IsAttacking", true);
            }

            if (events.attackMove && !attackMove)
            {
                perfectAttack = false;
                attackMoveDuration = 0.3f;
                attackMove = true;
                
                count++;
                if (count >= 3)
                {
                    count = 2;
                }
                AudioManager.Instance.PlaySFX(swordSFX.audioClips[count], swordSFX.audioCaption);
            }
            if (!events.attackMove)
            {
                attackMoveDuration = 0f;
                attackMove = false;
                
            }
        }
        if (isGhost)
        {
            AudioManager.Instance.FiltersOn();
            MovementInput();
        } else
        {
            AudioManager.Instance.FiltersOff();
        }
        if(!isGhost && !gameOver && !isFalling)
        {
            if(sanity < maxSanity)
            {
                sanityRegenCoolDown -= Time.deltaTime;
                if (sanityRegenCoolDown <= 0)
                {
                    sanity += sanityRegenAmount;
                    sanityRegenCoolDown = sanityRegenCoolDownMax;
                }
            }
            if(sanity > maxSanity)
            {
                sanity = maxSanity;
            }
        }
    }
    void MovementInput()
    {
        if (Input.GetKey(m_cBindings.Left))
        {
            moveAxis = new Vector2(-1, 0).normalized;
        }
        if (Input.GetKey(m_cBindings.Right))
        {
            moveAxis = new Vector2(1, 0).normalized;
        }
        if (Input.GetKey(m_cBindings.Up))
        {
            moveAxis = new Vector2(0, 1).normalized;
        }
        if (Input.GetKey(m_cBindings.Down))
        {
            moveAxis = new Vector2(0, -1).normalized;
        }
        if (Input.GetKey(m_cBindings.Right) && Input.GetKey(m_cBindings.Up))
        {
            moveAxis = new Vector2(1, 1).normalized;
        }
        if (Input.GetKey(m_cBindings.Left) && Input.GetKey(m_cBindings.Up))
        {
            moveAxis = new Vector2(-1, 1).normalized;
        }
        if (Input.GetKey(m_cBindings.Right) && Input.GetKey(m_cBindings.Down))
        {
            moveAxis = new Vector2(1, -1).normalized;
        }
        if (Input.GetKey(m_cBindings.Left) && Input.GetKey(m_cBindings.Down))
        {
            moveAxis = new Vector2(-1, -1).normalized;
        }
        if (Input.GetKeyUp(m_cBindings.Left) || Input.GetKeyUp(m_cBindings.Right)  || Input.GetKeyUp(m_cBindings.Up) || Input.GetKeyUp(m_cBindings.Down))
        {
            moveAxis = new Vector2(0, 0).normalized;
        }
    }
    private void FixedUpdate()
    {
        grb.velocity = Vector2.zero;
        rb.velocity = Vector2.zero;

        previousPosition = transform.position;
        //rb.MovePosition(rb.position + (moveAxis * moveSpeed * Time.fixedDeltaTime));
        if (!isGhost)
        {
            //rb.MovePosition(rb.position + (moveAxis * moveSpeed * 0.02f));
            rb.velocity = (moveAxis * moveSpeed) + knockbackVelocity;

            if (attackMove)
            {
                if (attackMoveDuration > 0)
                {
                    rb.MovePosition(rb.position + (facingAxis * 20 * attackMoveDuration * Time.fixedDeltaTime));
        
                    //rb.velocity = (moveAxis * moveSpeed);
                    attackMoveDuration -= Time.fixedDeltaTime;
                }
            }
        }
        else
        {
            if (!isPossess)
            {
                //grb.MovePosition(grb.position + (moveAxis * moveSpeed * Time.fixedDeltaTime));
                grb.velocity = (moveAxis * moveSpeed);
            }
        }

        if(knockbackVelocity.magnitude < 0.1f)
        {
            knockbackVelocity = Vector2.zero;
        }
        else
        {
            knockbackVelocity *= 0.9f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Floor")
        {
            //Debug.Log("On Floor");
            floorCollisions.Add(collision.gameObject);
        }
    }

    private void KnockBack()
    {
        knockbackVelocity = (transform.position - damage.lastDamagePos).normalized * knockbackAmount;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            if (floorCollisions.Contains(collision.gameObject))
            {
                floorCollisions.Remove(collision.gameObject);
            }
            if (floorCollisions.Count == 0)
            {
                Debug.Log("Fell Off");
                FallOffMap();
            }
        }
    }

    void FallOffMap()
    {
        if(moveAxis.y>=0) sr.sortingOrder = -1;
        canControl = false;
        isFalling = true;
        GetComponent<EnitySpriteSort>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(2).gameObject.SetActive(false);
        //respawn in x seconds
        
    }

    public void SwitchToPlayer()
    {
        ghost.transform.position = transform.position;
        animator.SetBool("IsDead", false);
        ghostAnimator.SetBool("Spawn", false);
        ghostAnimator.SetBool("Possess", true);
        isGhost = false;
        isPossess = false;
        tether.BreakTether();
    }

    public void SwitchToGhost()
    {
        ghost.transform.position = transform.position;
        animator.SetBool("IsDead", true);
        ghostAnimator.SetBool("Spawn", true);
        ghostAnimator.SetBool("Possess", false);
        isGhost = true;
        isPossess = false;
        tether.ShowTether(ghost.transform, transform);
        sanityRegenCoolDown = sanityRegenCoolDownMax;
    }

    public void GhostPossess()
    {
        //ghost.transform.position = transform.position;
        //animator.SetBool("IsDead", true);
        ghostAnimator.SetBool("Spawn", false);
        ghostAnimator.SetBool("Possess", true);
        isPossess = true;
        //isGhost = true;
    }
    public void GhostExit(Vector3 pos)
    {
        ghost.transform.position = pos;
        //animator.SetBool("IsDead", true);
        ghostAnimator.SetBool("Spawn", true);
        ghostAnimator.SetBool("Possess", false);
        isPossess = false;
        //isGhost = true;

    }
    public void SetMoveSpeed(float _amount)
    {
        moveSpeed = _amount;
    }
}
