using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
///Picks which menu to open based on context
public class MenuDecider : MonoBehaviour
{
    [SerializeField]
    GameObject m_pauseMenu;
    [SerializeField]
    GameObject m_mainMenu;
    [SerializeField]
    GameObject m_optionsMenu;

    [SerializeField]
    Button m_backButton;
    [SerializeField]
    bool m_returnToMain;
    void Start()
    {
        m_backButton.onClick.AddListener(GoBack);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            m_returnToMain = true;
        }
        else
            m_returnToMain = false;
    }
    void GoBack()
    {
        if (m_returnToMain)
        {
            m_mainMenu.SetActive(true);
            m_optionsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(m_mainMenu.transform.GetChild(2).gameObject);
            m_backButton.gameObject.GetComponent<ResizeButton>().ResetButtonHighlight(true);
        }
        if (!m_returnToMain)
        {
            m_pauseMenu.SetActive(true);
            m_optionsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(m_pauseMenu.transform.GetChild(2).gameObject);
            m_backButton.gameObject.GetComponent<ResizeButton>().ResetButtonHighlight(true);
        }
    }

}
