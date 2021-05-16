using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum KeyToBind
{
    Up,
    Down,
    Left,
    Right,
    Interact,
    Switch,
}
public class ControlBindings : MonoBehaviour
{
    public KeyCode Up;
    public KeyCode Left;
    public KeyCode Down;
    public KeyCode Right;
    public KeyCode Interact;
    public KeyCode SwitchForm;
    public KeyCode Temp;
    public KeyToBind KeyToBindVariable;
    bool m_shouldRebind;
    public GameObject RebindButton;
    public TextMeshProUGUI[] InputDisplayTexts;
    void Start()
    {
        LoadControls();
        SetInputDisplayText();
    }
    void Update()
    {
        if (Temp == KeyCode.Return)
            Temp = KeyCode.None;
        if (m_shouldRebind)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    Temp = vKey;
                    if (Temp != KeyCode.Return)
                    {
                        RebindButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Rebind " + KeyToBindVariable + " to";
                        RebindButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Temp.ToString();
                    }
                }
            }
        }
    }
    public void SetInputDisplayText()
    {
        InputDisplayTexts[0].text = "= " + Up.ToString();
        InputDisplayTexts[1].text = "= " + Down.ToString();
        InputDisplayTexts[2].text = "= " + Left.ToString();
        InputDisplayTexts[3].text = "= " + Right.ToString();
        InputDisplayTexts[4].text = "= " + SwitchForm.ToString();
        InputDisplayTexts[5].text = "= " + Interact.ToString();
    }
    public void DisplayCorrectPrompt()
    {
        RebindButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Rebind " + KeyToBindVariable + " to";
        switch (KeyToBindVariable)
        {
            case KeyToBind.Up:
                RebindButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Up.ToString();
                m_shouldRebind = false;
                break;
            case KeyToBind.Down:
                RebindButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Down.ToString();
                m_shouldRebind = false;
                break;
            case KeyToBind.Left:
                RebindButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Left.ToString();
                m_shouldRebind = false;
                break;
            case KeyToBind.Right:
                RebindButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Right.ToString();
                m_shouldRebind = false;
                break;
            case KeyToBind.Interact:
                RebindButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Interact.ToString();
                m_shouldRebind = false;
                break;
            case KeyToBind.Switch:
                RebindButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SwitchForm.ToString();
                m_shouldRebind = false;
                break;
        }
    }
    public void BindKey()
    {
        if (Temp != Up && Temp != Down && Temp != Right && Temp != Left && Temp != Interact && Temp != SwitchForm)
        {
            switch (KeyToBindVariable)
            {
                case KeyToBind.Up:
                    Up = Temp;
                    m_shouldRebind = false;
                    break;
                case KeyToBind.Down:
                    Down = Temp;
                    m_shouldRebind = false;
                    break;
                case KeyToBind.Left:
                    Left = Temp;
                    m_shouldRebind = false;
                    break;
                case KeyToBind.Right:
                    Right = Temp;
                    m_shouldRebind = false;
                    break;
                case KeyToBind.Interact:
                    Interact = Temp;
                    m_shouldRebind = false;
                    break;
                case KeyToBind.Switch:
                    SwitchForm = Temp;
                    m_shouldRebind = false;
                    break;
            }
        }
    }
    public void SetKeyToBindEnum(int _value)
    {
        KeyToBindVariable = (KeyToBind)_value;
        RebindButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Rebind " + KeyToBindVariable;
        m_shouldRebind = true;
    }
    public void LoadControls()
    {
        if (PlayerPrefs.GetString("Up") != "")
            Up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up"));
        if (PlayerPrefs.GetString("Down") != "")
            Down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down"));
        if (PlayerPrefs.GetString("Left") != "")
            Left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left"));
        if (PlayerPrefs.GetString("Right") != "")
            Right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right"));
        if (PlayerPrefs.GetString("Interact") != "")
            Interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));
        if (PlayerPrefs.GetString("Switch") != "")
            SwitchForm = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Switch"));
    }
    public void SaveControls()
    {
        PlayerPrefs.SetString("Up", Up.ToString());
        PlayerPrefs.SetString("Down", Down.ToString());
        PlayerPrefs.SetString("Left", Left.ToString());
        PlayerPrefs.SetString("Right", Right.ToString());
        PlayerPrefs.SetString("Interact", Interact.ToString());
        PlayerPrefs.SetString("Switch", SwitchForm.ToString());
    }
}
