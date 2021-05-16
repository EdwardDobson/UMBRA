using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGPUI;
using System.Linq;

public class PossessInteract : MonoBehaviour
{
    [SerializeField]
    PlayerController m_pController;
    ControlBindings m_cBindings;
    [SerializeField]
    SwitchForm m_switchForm;
    [SerializeField]
    bool m_isPossessing;
    [SerializeField]
    float m_sanityDrainTimer;
    [SerializeField]
    float m_saintyDrainAmount;
    [SerializeField]
    GameObject m_possessedObj;
    bool m_showInteractPopup;
    [SerializeField]
    float m_posOffset;
    Object[] PossessableObjects;
    private void Start()
    {
        m_cBindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
        PossessableObjects = FindObjectsOfType(typeof(PossessedObject));
    }
    private void Update()
    {
        if (m_pController.gameOver) return;

        DrainSanity();
        if (Input.GetButtonDown("ButtonA") || Input.GetKeyDown(m_cBindings.SwitchForm) && m_pController.sanity > 1)
        {
            if (m_possessedObj != null)
            {
                if (m_isPossessing)
                {
                    foreach (PossessedObject g in PossessableObjects)
                    {
                        g.SetPossessedState(false);
                    }
                    m_isPossessing = false;
                    m_pController.tether.ShowTether(m_pController.ghost.transform, m_pController.transform);
                    m_pController.GhostExit(m_possessedObj.transform.position);
                    m_possessedObj.GetComponent<PossessedObject>().SetPossessedState(false);
                    m_possessedObj = null;
                }
                else
                {
                    m_isPossessing = true;
                    m_possessedObj.GetComponent<PossessedObject>().SetPossessedState(true);
                    m_showInteractPopup = false;
                    m_pController.tether.ShowTether(m_possessedObj.transform, m_pController.transform);
                    m_pController.GhostPossess();
                }
            }
        }
        if (!m_switchForm.IsInGhostForm())
        {
            
            if(m_possessedObj != null)
            {
                m_possessedObj.GetComponent<PossessedObject>().SetPossessedState(false);
                m_possessedObj = null;
            }
     
            m_isPossessing = false;
        }
        if(m_showInteractPopup && !m_isPossessing)
        {
            DialogBoxes.DisplayButtonPrompt(transform.position, m_posOffset, m_cBindings.SwitchForm.ToString());
        }
        else
        {
            DialogBoxes.HideButtonPrompt();
        }
    }
    void DrainSanity()
    {
        if (m_isPossessing)
        {
            m_sanityDrainTimer -= Time.deltaTime;
            if (m_sanityDrainTimer <= 0)
            {
                m_sanityDrainTimer = 1f;
                m_pController.sanity -= m_saintyDrainAmount;
            }
            if (m_possessedObj != null)
            {
                if(m_possessedObj.GetComponent<BaseEnemy>() != null)
                {
                    if (m_possessedObj.GetComponent<BaseEnemy>().health <= 0)
                    {
                        m_pController.transform.parent.GetChild(2).transform.GetComponent<TetherScript>().ghostTransform = m_pController.transform.parent.GetChild(1).transform;
                        m_pController.GhostExit(m_possessedObj.transform.position);
                    }
                }
            }
            if (m_pController.sanity <= 1)
            {
                if (m_possessedObj != null)
                {
                    m_pController.GhostExit(m_possessedObj.transform.position);
                    m_possessedObj.GetComponent<PossessedObject>().SetPossessedState(false);
                    m_possessedObj = null;
                }

                foreach (PossessedObject g in PossessableObjects)
                {
                    g.SetPossessedState(false);
                }
                m_isPossessing = false;
                m_pController.tether.ShowTether(m_pController.ghost.transform, m_pController.transform);
                m_pController.ghost.transform.position = new Vector2(m_pController.ghost.transform.position.x + 1, m_pController.ghost.transform.position.y);
                m_pController.sanity = 1;
            }
        }
  
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_pController.gameOver)
        {
            m_showInteractPopup = false;
            return;
        }
        if (collision.gameObject.tag.Contains("Enemy") || collision.gameObject.tag.Contains("Possessable") && !m_isPossessing)
        {
            if (collision.gameObject.GetComponent<PossessedObject>())
            {
                if (m_switchForm.IsInGhostForm() && collision.gameObject.GetComponent<PossessedObject>().GetIsPossessableState())
                {
                    m_possessedObj = collision.gameObject;
                    m_showInteractPopup = true;

                }
                else
                {
                    m_possessedObj = null;
                    m_showInteractPopup = false;
                }
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_pController.gameOver)
        {
            m_showInteractPopup = false;
            return;
        }

        if (collision.gameObject.tag.Contains("Enemy") || collision.gameObject.tag.Contains("Possessable") && !m_isPossessing)
        {
            if (collision.gameObject.GetComponent<PossessedObject>())
            {
                if (m_switchForm.IsInGhostForm())
                {
                    m_possessedObj = collision.gameObject;
                    m_showInteractPopup = true;
                }
                else
                {
                    m_possessedObj = null;
                    m_showInteractPopup = false;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_pController.gameOver)
        {
            m_showInteractPopup = false;
            return;
        }

        if (collision.gameObject.tag.Contains("Enemy") || collision.gameObject.tag.Contains("Possessable") && !m_isPossessing)
        {
            if (collision.gameObject.GetComponent<PossessedObject>())
            {
                m_showInteractPopup = false;
                if (!m_isPossessing)
                    m_possessedObj = null;
            }
        }
    }

    public bool GetShowPopUpState()
    {
        return m_showInteractPopup;
    }
    public bool GetPossessingState()
    {
        return m_isPossessing;
    }
}
