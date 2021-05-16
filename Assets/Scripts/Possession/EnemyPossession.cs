using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyPossession : PossessedObject
{

    public float MoveSpeed;
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (IsPossessed)
        {
            MovementInput();
        }

    }
    private void FixedUpdate()
    {
        if (IsPossessed)
        {
            if(m_rigidbody2D != null)
            {
                m_rigidbody2D = GetComponent<Rigidbody2D>();
                Movement(m_rigidbody2D, MoveSpeed);
            }

        }
    }
}
