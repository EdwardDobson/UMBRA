using AGPUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ExitType
{
    Interact,
    Enter
}
public class WinScreen : MonoBehaviour
{
    public GameObject WinScreenObj;
    public ExitType ExitType;
    ControlBindings m_controlBindings;
    [SerializeField]
  public  bool inZone;
    void Start()
    {
        m_controlBindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
        WinScreenObj = GameObject.Find("GameManager").transform.GetChild(9).gameObject;
    }
    void Update()
    {
        if(inZone)
        {
            if (ExitType == ExitType.Interact)
            {
                DialogBoxes.DisplayButtonPrompt(transform.position, 150, m_controlBindings.Interact.ToString());
                if (Input.GetKeyDown(m_controlBindings.Interact))
                {
                    DisplayWinScreen();
                }
            }
        }
    }
    void DisplayWinScreen()
    {
        WinScreenObj.SetActive(true);
        Time.timeScale = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            inZone = true;
            if (ExitType == ExitType.Enter)
            {
                DisplayWinScreen();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inZone = false;
        }
    }
}
