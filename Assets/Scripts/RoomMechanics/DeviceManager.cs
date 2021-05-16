using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{
    public int TotalWorth;
    public int RequiredAmount;
    public bool PerformAction;
    public Side ShowSide;
    SwitchForm m_form;
    public GameObject[] ObjectsToHide;
    public GameObject CameraFocus;
    private void Start()
    {
        m_form = GameObject.Find("Player").GetComponent<SwitchForm>();
    }
    private void Update()
    {
        if (TotalWorth == RequiredAmount)
        {
            PerformAction = true;
            if (CameraFocus != null)
            {
                CameraFocus.SetActive(true);
                CameraFocus = null;
            }

        }
        else PerformAction = false;
        HideObjects();

        if (TotalWorth < 0)
            TotalWorth = 0;

    }
    void HideObjects()
    {
        foreach (GameObject t in ObjectsToHide)
        {
            if (t.GetComponent<Device>())
            {
                if (m_form.transform.parent.GetChild(0).GetComponent<SwitchForm>().IsInGhostForm())
                {
                    if (t.GetComponent<Device>().Form == Side.Ghost)
                    {
                        t.GetComponent<BoxCollider2D>().enabled = true;
                        t.GetComponent<SpriteRenderer>().enabled = true;
                    }
                    if (t.GetComponent<Device>().Form == Side.Player)
                    {
                        t.GetComponent<BoxCollider2D>().enabled = false;
                        t.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                if (!m_form.transform.GetComponent<SwitchForm>().IsInGhostForm())
                {
                    if (t.GetComponent<Device>().Form == Side.Player)
                    {
                        t.GetComponent<BoxCollider2D>().enabled = true;
                        t.GetComponent<SpriteRenderer>().enabled = true;
                    }
                    if (t.GetComponent<Device>().Form == Side.Ghost)
                    {
                        t.GetComponent<BoxCollider2D>().enabled = false;
                        t.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
        }
    }
}
