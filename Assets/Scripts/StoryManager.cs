using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
   static StoryManager m_instance;
    public List<int> FinishedStories = new List<int>();
    [SerializeField]
    StoryTrigger[] m_triggers;
    void Start()
    {
        DontDestroyOnLoad(this);

        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            m_triggers = FindObjectsOfType<StoryTrigger>();
            for (int i = 0; i < m_triggers.Length; ++i)
            {
                for (int a = 0; a < FinishedStories.Count; ++a)
                {
                    if (m_triggers[i].StoryPiece.ID == FinishedStories[a])
                    {
                        m_triggers[i].transform.gameObject.SetActive(false);
                        Debug.Log("Hide used story bits");
                    }
                }
            }
        }
        else
        {
            FinishedStories.Clear();
        }
   
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
