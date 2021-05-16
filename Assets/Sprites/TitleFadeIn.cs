using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleFadeIn : MonoBehaviour
{
    public float FadeInSpeed;
    TextMeshProUGUI m_title;
    public GameObject[] MainMenuButtons;
    public Animator m_animator;
    [SerializeField]
    bool m_shouldPlay;
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayAnimation();
        }
    }
    public void PlayAnimation()
    {
        foreach (GameObject g in MainMenuButtons)
        {
            g.SetActive(false);
        }
        Time.timeScale = 1;
        m_shouldPlay = true;
        StartCoroutine(TitleAnimation());
    }

    IEnumerator TitleAnimation()
    {
        m_title = GetComponent<TextMeshProUGUI>();
        m_animator = GetComponent<Animator>();
        int tVisibleCharacters = m_title.text.Length;
        int counter = 0;

        while (true)
        {
            if (m_shouldPlay)
                m_animator.Play("Main Title");
            int visCount = counter % (tVisibleCharacters + 1);
            m_title.maxVisibleCharacters = visCount;

            counter += 1;
            yield return new WaitForSeconds(FadeInSpeed);
            if (visCount >= tVisibleCharacters)
            {
                foreach(GameObject g in MainMenuButtons)
                {
                    g.SetActive(true);
                    m_shouldPlay = false;
                }
                yield return null;
                break;
            }
        }
    }
}
