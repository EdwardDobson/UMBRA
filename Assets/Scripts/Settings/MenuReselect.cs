using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MenuReselect : MonoBehaviour
{
    [SerializeField]
    GameObject m_selectedButton;
    [SerializeField]
    GameObject m_lastSelectedButton;

    public AudioClip menuBlipNoise;
    void Start()
    {
        m_selectedButton = EventSystem.current.currentSelectedGameObject;
    }
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != m_selectedButton)
        {
            AudioManager.Instance.PlaySFX(menuBlipNoise);
            m_lastSelectedButton = m_selectedButton;
            m_selectedButton = EventSystem.current.currentSelectedGameObject;            
        }
        if (m_selectedButton == null && m_lastSelectedButton != null)
            EventSystem.current.SetSelectedGameObject(m_lastSelectedButton);
    }
}
