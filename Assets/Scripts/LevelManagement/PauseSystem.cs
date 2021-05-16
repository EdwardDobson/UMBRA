using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PauseSystem : MonoBehaviour
{
    GameObject m_pauseMenu;
    [SerializeField]
    GameObject m_optionsMenu;
    [SerializeField]
    GameObject m_graphicsMenu;
    [SerializeField]
    GameObject m_audioMenu;
    [SerializeField]
    GameObject m_controlsMenu;
    [SerializeField]
    GameObject m_gameOverMenu;
    [SerializeField]
    GameObject m_mainMenu;
    [SerializeField]
    GameObject m_winMenu;
    public bool IsPaused;
    void Start()
    {
        m_pauseMenu = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_optionsMenu.activeSelf && !m_graphicsMenu.activeSelf && !m_audioMenu.activeSelf && !m_controlsMenu.activeSelf && !m_gameOverMenu.activeSelf && !m_winMenu.activeSelf)
        {
            if (IsPaused)
                Resume();
            else if (!IsPaused && !m_mainMenu.activeSelf)
                Pause();
        }
        if (m_mainMenu != null)
        {
            if (m_mainMenu.activeSelf)
            {
                m_pauseMenu.SetActive(false);
            }
        }
    }
    public void Pause()
    {
        Debug.Log("paused");
        m_pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(2).gameObject);
        IsPaused = true;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        m_pauseMenu.SetActive(false);
        IsPaused = false;
        Time.timeScale = 1;
    }
}
