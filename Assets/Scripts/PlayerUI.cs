using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using AGPUI;
public class PlayerUI : MonoBehaviour
{
    Slider m_sanityBar;
    TextMeshProUGUI m_sanityValue;
    [SerializeField]
    TextMeshProUGUI SoulsText;
    PlayerController m_pController;
    PossessInteract m_pInteract;
    SwitchForm m_switchForm;
    Image m_sanityBarImage;
    UnityEngine.Experimental.Rendering.Universal.Light2D m_globalLight;
    public DamagePopup DamagePopup;
    public Image DeathPanelImage;
    float m_deathTimer = 0f;

    WinScreen m_winScreen;
    [SerializeField]
    GameObject m_possessingIconAnimation;
    UnityEngine.Rendering.VolumeProfile m_profile;
    UnityEngine.Rendering.Universal.Vignette m_vignette;

    [SerializeField]
    Color m_vignetteColourDay = new Color();
    [SerializeField]
    Color m_lightColourDay = new Color();
    [SerializeField]
    Color m_vignetteColourNight = new Color();
    [SerializeField]
    Color m_lightColourNight = new Color();
    [SerializeField]
    Color m_sanityBarNightColour = new Color();
    [SerializeField]
    Color m_sanityBarDayColour = new Color();
    void Start()
    {
        m_pController = GameObject.Find("Player").GetComponent<PlayerController>();
        m_pInteract = GameObject.Find("Ghost").GetComponent<PossessInteract>();
        m_switchForm = GameObject.Find("Player").GetComponent<SwitchForm>();

        m_sanityBar = transform.GetChild(0).GetComponent<Slider>();
        m_sanityBar.maxValue = m_pController.maxSanity;

        m_sanityValue = transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();

        m_sanityBarImage = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();
        m_sanityBarImage.color = new Color(255, 243, 81, 255);

        m_profile = GameObject.FindGameObjectWithTag("Post-Processing Volume").GetComponent<UnityEngine.Rendering.Volume>().profile;
        m_globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        m_profile.TryGet(out m_vignette);

        m_possessingIconAnimation = transform.GetChild(0).GetChild(0).gameObject;

        DialogBoxes.DialogBoxesSetup();

        DamagePopup.DamagePopSetUp();
        //     DamagePopup.Spawn(m_pController.transform.position,10,1f);
        if(GameObject.Find("World Exit") != null)
        m_winScreen = GameObject.Find("World Exit").GetComponent<WinScreen>();
    }
    void Update()
    {
        UpdateSanityUI();
        if (m_switchForm.IsInGhostForm())
        {
            m_sanityBarImage.color = m_sanityBarNightColour;
            m_vignette.color.value = m_vignetteColourNight;
            m_globalLight.color = m_lightColourNight;
            m_possessingIconAnimation.SetActive(true);
        }
        else
        {
            m_vignette.color.value = m_vignetteColourDay;
            m_globalLight.color = m_lightColourDay;
            m_sanityBarImage.color = m_sanityBarDayColour;
            m_possessingIconAnimation.SetActive(false);
        }
        if(m_winScreen != null)
        {
            if (!m_pInteract.GetShowPopUpState() && !m_winScreen.inZone)
            {
                DialogBoxes.HideButtonPrompt();
                DialogBoxes.HideDisplayTip();
            }
        }
    
        if (m_pController.sanity <= 0)
        {
            DeathPanelImage.color = new Color(DeathPanelImage.color.r, DeathPanelImage.color.g, DeathPanelImage.color.b, m_deathTimer);
            m_deathTimer += 0.5f * Time.deltaTime;
            if (m_deathTimer >= 1f)
            {
                m_deathTimer = 1f;
                //open gameover
                GameoverManager.Instance.go.SetActive(true);
            }
        }
    }
    public void UpdateSanityUI()
    {
        m_sanityBar.maxValue = m_pController.maxSanity;
        m_sanityBar.value = m_pController.sanity;
        float sBarValue = m_pController.sanity / m_pController.maxSanity * 100;
        if (m_pController.sanity >= 0)
            m_sanityValue.text = (int)sBarValue + " %";
        else
            m_sanityValue.text = "0 %";
        SoulsText.text = "x " + m_pController.souls;
    }
}
