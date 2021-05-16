using AGPUI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
/// <summary>
/// Levers, Pressure plates, buttons
/// </summary>
public class Device : MonoBehaviour
{
    public bool NeedsPlayerInput;
    public bool NeedsTimer;
    public float MaxTimer;
    public float CurrentTimer;
    public Side Form;
    public DeviceManager[] Managers;
    [SerializeField]
    bool m_deviceActive;
    [SerializeField]
    bool m_nearPlayer;
    ControlBindings m_cBindings;
    [SerializeField]
    int m_amountInTrigger = 2;
    bool m_startTimer;
    [SerializeField]
    bool m_objectOnTop;
    SwitchForm m_switchForm;
    private void Start()
    {
        m_cBindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
        m_switchForm = GameObject.Find("Player").GetComponent<SwitchForm>();
    }
    public int WorthToGive;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains(Form.ToString()))
        {

            if (Form == Side.Player && !collision.GetComponent<SwitchForm>().IsInGhostForm())
            {
                if (!m_deviceActive)
                {
                    if (!NeedsPlayerInput)
                        On();
                    if (NeedsTimer && !NeedsPlayerInput)
                        m_startTimer = false;
                }
            }
        }
        if (Form == Side.Both)
        {
            if (collision.gameObject.tag.Contains("Player"))
            {
                if (m_amountInTrigger == 0)
                    m_amountInTrigger = 2;
                if (m_amountInTrigger < 2)
                    m_amountInTrigger++;
                if (!m_deviceActive)
                {
                    if (!NeedsPlayerInput)
                    {
                        On();
                    }
                    if (NeedsTimer && !NeedsPlayerInput)
                        m_startTimer = false;
                }
            }
        }
        if (collision.gameObject.tag.Contains("Possessable"))
        {
            if (collision.gameObject.GetComponent<PossessedObject>().InteractsWithDevices)
            {
                if (!m_deviceActive)
                {
                    if (!NeedsPlayerInput)
                    {
                        On();
                    }
                    if (NeedsTimer && !NeedsPlayerInput)
                        m_startTimer = false;
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains(Form.ToString()))
        {
            if (Form == Side.Ghost && collision.transform.parent.GetChild(0).GetComponent<SwitchForm>().IsInGhostForm())
            {
                if (NeedsPlayerInput)
                    m_nearPlayer = true;
            }
            if (Form == Side.Player && !collision.GetComponent<SwitchForm>().IsInGhostForm())
            {
                if (NeedsPlayerInput)
                    m_nearPlayer = true;
                if (NeedsTimer && !NeedsPlayerInput)
                    m_startTimer = false;
            }
        }
        if (Form == Side.Both)
        {
            if (collision.gameObject.tag.Contains("Player"))
            {
                if (NeedsPlayerInput)
                    m_nearPlayer = true;
                if (NeedsTimer && !NeedsPlayerInput)
                    m_startTimer = false;
            }
        }
        if (collision.gameObject.tag.Contains("Possessable"))
        {
            if (collision.gameObject.GetComponent<PossessedObject>().InteractsWithDevices)
            {
                if (NeedsTimer && !NeedsPlayerInput)
                    m_startTimer = false;
                m_objectOnTop = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!m_objectOnTop)
        {
            if (collision.gameObject.name.Contains(Form.ToString()))
            {
                if (NeedsPlayerInput)
                {
                    m_nearPlayer = false;
                }
            }
            if (collision.gameObject.name.Contains("Player"))
            {
                if (!NeedsPlayerInput && !NeedsTimer)
                {
                    Off();
                }
                if (!NeedsPlayerInput && NeedsTimer)
                {
                    m_startTimer = true;
                }
            }
            if (Form == Side.Both)
            {
                if (collision.gameObject.tag.Contains("Player"))
                {
                    if (NeedsPlayerInput)
                    {
                        m_nearPlayer = false;
                    }
                    else if (!NeedsPlayerInput && !NeedsTimer)
                    {
                        m_amountInTrigger -= 2;

                        if (m_amountInTrigger <= 0)
                            Off();
                    }
                    if (!NeedsPlayerInput && NeedsTimer)
                    {
                        m_startTimer = true;
                    }
                }
            }
        }

        if (collision.gameObject.tag.Contains("Possessable"))
        {
            if (collision.gameObject.GetComponent<PossessedObject>().InteractsWithDevices)
            {
                if (!NeedsPlayerInput && !NeedsTimer)
                {
                    Off();
                }
                if (!NeedsPlayerInput && NeedsTimer)
                {
                    m_startTimer = true;
                    m_objectOnTop = false;
                }

            }
        }
    }
    private void Update()
    {
        if (m_startTimer)
            OffTimer();
        InputFunction();
        if (m_amountInTrigger < 0)
            m_amountInTrigger = 0;   
    
    }
    void InputFunction()
    {
        if (m_nearPlayer)
        {
            if (!m_switchForm.IsInGhostForm())
            {
                if (Input.GetKeyDown(m_cBindings.Interact) || Input.GetButtonDown("ButtonA"))
                {
                    if (!m_deviceActive)
                        On();
                    else
                        Off();
                }
            }
            else
            {
                if (GetComponent<PossessedObject>() != null)
                {
                    if (GetComponent<PossessedObject>().GetPossessedState())
                    {
                        DialogBoxes.DisplayButtonPrompt(transform.position, 150, m_cBindings.Interact.ToString());
                        if (Input.GetKeyDown(m_cBindings.Interact) || Input.GetButtonDown("ButtonA"))
                        {
                            if (!m_deviceActive)
                                On();
                            else
                                Off();
                        }
                    }
                }
            }
        }
    }
    //Functions for increasing/decreasing the total worth
    void On()
    {
        for (int i = 0; i < Managers.Length; ++i)
            Managers[i].TotalWorth += WorthToGive;
        m_deviceActive = true;
        GetComponent<SpriteRenderer>().color = Color.green;
        GetComponent<SpriteRenderer>().flipX = true;
    }
    void Off()
    {
        for (int i = 0; i < Managers.Length; ++i)
            Managers[i].TotalWorth -= WorthToGive;
        m_deviceActive = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().flipX = false;
    }
    void OffTimer()
    {
        if (m_deviceActive && NeedsTimer)
        {
            CurrentTimer += Time.deltaTime;
            if (CurrentTimer >= MaxTimer)
            {
                for (int i = 0; i < Managers.Length; ++i)
                    Managers[i].TotalWorth -= WorthToGive;
                CurrentTimer = 0;
                m_deviceActive = false;
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
