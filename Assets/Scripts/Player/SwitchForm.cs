using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwitchForm : MonoBehaviour
{
    [SerializeField]
    bool m_inGhostForm;
    ControlBindings m_cBindings;
    PlayerController m_controller;
    GameObject m_ghost;
    [SerializeField]
    float m_recallCurrentTimer;
    public float MaxRecallTimer;
    GameObject m_tetherObj;
    PauseSystem m_pauseSystem;
    bool m_hitGhostForm;
    void Start()
    {
        m_recallCurrentTimer = MaxRecallTimer;
        m_cBindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
        m_pauseSystem = GameObject.Find("Pause").GetComponent<PauseSystem>();
        m_controller = GetComponent<PlayerController>();
        m_ghost = GameObject.Find("Ghost");
        m_tetherObj = GameObject.Find("Tether");

    }
    void Update()
    {
        if (Input.GetKey(m_cBindings.SwitchForm) || Input.GetButton("ButtonB"))
        {
            if (m_inGhostForm)

            {
                foreach (Transform t in m_tetherObj.transform)
                {
                    Color c = Color.Lerp(Color.white, Color.black, m_recallCurrentTimer);
                    ChangeTetherColour(c, Color.black, m_recallCurrentTimer, t.GetComponent<ParticleSystem>());
                }

                m_recallCurrentTimer -= Time.deltaTime;
                if (m_recallCurrentTimer <= 0)
                {

                    m_recallCurrentTimer = MaxRecallTimer;
                    ChangeToPlayer();
                }
            }
        }
        if (Input.GetKeyUp(m_cBindings.SwitchForm) || Input.GetButtonUp("ButtonB"))
        {
            if (m_inGhostForm)
            {
                m_recallCurrentTimer = MaxRecallTimer;
                foreach (Transform t in m_tetherObj.transform)
                {
                    ChangeTetherColour(Color.black, Color.black, 0, t.GetComponent<ParticleSystem>());
                }

            }
        }
        if (Input.GetKeyDown(m_cBindings.SwitchForm) || Input.GetButtonDown("ButtonB"))
        {
            if (!m_hitGhostForm)
            {
                if (!m_inGhostForm)
                {
                    foreach (Transform t in m_tetherObj.transform)
                    {
                        ChangeTetherColour(Color.black, Color.black, 0, t.GetComponent<ParticleSystem>());
                    }
                    ChangeToGhost();
                }
            }
            else
            {
                ChangeToPlayer();
                m_hitGhostForm = false;
            }
        }
        if (!m_inGhostForm)
        {
            m_ghost.transform.position = transform.position;
        }
    }
    void ChangeTetherColour(Color _startColour, Color _endColour, float _increaseValue, ParticleSystem _system)
    {
        ParticleSystem.ColorOverLifetimeModule m_gradChangeColour = _system.colorOverLifetime;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(_startColour, 1.0f), new GradientColorKey(_endColour, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        m_gradChangeColour.color = grad;
    }
    void ChangeToGhost()
    {
        if(!m_pauseSystem.IsPaused && Time.timeScale > 0)
        {
            m_controller.SwitchToGhost();
            m_inGhostForm = true;
        }
    }

    public void ChangeToPlayer()
    {
        m_controller.SwitchToPlayer();
        m_inGhostForm = false;
    }

    public bool IsInGhostForm()
    {
        return m_inGhostForm;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Ghost") && m_inGhostForm)
        {
            m_hitGhostForm = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Ghost"))
        {
            m_hitGhostForm = false;
        }
    }
}
