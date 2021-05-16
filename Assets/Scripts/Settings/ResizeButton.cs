using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
///Class to resize and modify the menu buttons
public class ResizeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    Button m_button;
    Image m_image;

    #region Dimensions
    RectTransform m_buttonTransform;
    [SerializeField]
    Vector2 m_scale = new Vector2();
    Vector2 m_defaultScale = new Vector2();
    [SerializeField]
    bool m_isMainButton;
    #endregion

    #region Fade
    [SerializeField]
    float m_fadeInSpeed;
    [SerializeField]
    Color m_startButtonColour;
    [SerializeField]
    Color m_endButtonColour;
    float m_alpha;
    [SerializeField]
    bool m_shouldFade;
    #endregion
    [SerializeField]
    Color m_defaultColour;

    void Start()
    {
        GetComponents();
        m_image.color = m_startButtonColour;
        m_defaultScale = m_buttonTransform.localScale;
        if (m_shouldFade)
        {
            m_button.interactable = false;
            StartCoroutine("FadeIn");
        }
        else
        {
            m_button.interactable = true;
        }
        if (m_isMainButton)
            ResetButtonHighlight(false);
    }

    public void OnPointerEnter(PointerEventData _data)
    {
        ResetButtonHighlight(false);
    }
    public void OnPointerExit(PointerEventData _data)
    {
        ResetButtonHighlight(true);
    }
    public void OnSelect(BaseEventData _data)
    {
        ResetButtonHighlight(false);
    }
    public void OnDeselect(BaseEventData _data)
    {
        ResetButtonHighlight(true);
    }

    IEnumerator FadeIn()
    {
        m_alpha = 0;
        while (m_image.color.a <= 255)
        {
            m_alpha += Time.unscaledDeltaTime / m_fadeInSpeed;
            m_image.color = new Color(255, 255, 255, m_alpha);
            ResetButtonColour();
            yield return null;
        }
        yield return null;
    }
    void OnEnable()
    {
        GetComponents();
        if (m_shouldFade)
        {
            m_button.interactable = false;
            StartCoroutine("FadeIn");
        }
        else
        {
            m_button.interactable = true;
        }
    }
    void GetComponents()
    {
        m_button = GetComponent<Button>();
        m_image = GetComponent<Image>();
        m_buttonTransform = GetComponent<RectTransform>();
    }
    void OnDisable()
    {
        GetComponents();
        m_image.color = m_startButtonColour;
        m_buttonTransform.localScale = m_defaultScale;
    }
    void ResetButtonColour()
    {
        if (m_image.color.a >= 1)
        {
            m_button.interactable = true;
            m_image.color = m_endButtonColour;
            if (m_isMainButton)
            {
                ResetButtonHighlight(false);
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
            StopCoroutine("FadeIn");
        }
    }
    public void ResetButtonHighlight(bool _reset)
    {
        if (m_button != null)
        {
            if (m_button.interactable)
            {
                if (_reset)
                {
                    m_buttonTransform.localScale = m_defaultScale;
                    m_image.color = m_endButtonColour;
                }
                else
                {
                    m_buttonTransform.localScale = m_scale;
                    m_image.color = m_defaultColour;
                }
            }
        }
    }
}
