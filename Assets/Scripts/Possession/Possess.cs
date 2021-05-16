using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possess : MonoBehaviour
{
    [SerializeField]
    bool m_isPossessed = false;
    Vector2 moveAxis;
    public Rigidbody2D rb;
    public float moveSpeed; //This is a temp value for now until we decide on the move speed;
    void Update()
    {
        if(m_isPossessed)
        {
            moveAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
        else
        {
            moveAxis = new Vector2(0, 0);
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (moveAxis * moveSpeed * Time.fixedDeltaTime));
    }
    public void SetPossessedState(bool _state)
    {
        m_isPossessed = _state;
    }
}
