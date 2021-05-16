using AGPUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedObject : MonoBehaviour
{
    [SerializeField]
    protected bool IsPossessable;
    [SerializeField]
    protected bool IsPossessed;
    public bool ShouldPerformAction;
    [SerializeField]
    protected bool HasAnotherAction;
    public bool InteractsWithDevices;
    public ControlBindings m_cBindings;
    public Rigidbody2D m_rigidbody2D;
    public float Speed;
    Vector2 m_moveAxis;
    private void Start()
    {
        m_cBindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
    }
    void Update()
    {
        if (IsPossessed)
        {
            if (GetComponent<Rigidbody2D>() == null)
            {
                gameObject.AddComponent<Rigidbody2D>();
                m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                m_rigidbody2D.freezeRotation = true;
                m_rigidbody2D.gravityScale = 0;
            }
            MovementInput();
            Movement(m_rigidbody2D, Speed);
        }
        if (!IsPossessed)
        {
            if (m_rigidbody2D != null)
                Destroy(GetComponent<Rigidbody2D>());
        }
    }
    public void SetPossessedState(bool _state)
    {
        IsPossessed = _state;
    }
    public bool GetPossessedState()
    {
       return  IsPossessed;
    }
    public bool GetIsPossessableState()
    {
        return IsPossessable;
    }
    protected void MovementInput()
    {
        m_moveAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    protected void Movement(Rigidbody2D _rb,float _moveSpeed)
    {
        _rb.MovePosition(_rb.position + (m_moveAxis * _moveSpeed * Time.fixedDeltaTime));
    }
    public void PerformAction()
    {
        if(m_cBindings == null)
        m_cBindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
        if (Input.GetKeyDown(m_cBindings.Interact) || Input.GetButtonDown("ButtonA"))
        {
            ShouldPerformAction = true;
        }
        if (Input.GetKeyUp(m_cBindings.Interact) || Input.GetButtonUp("ButtonA"))
        {
            ShouldPerformAction = false;
        }
    }

}
