using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
public class Settings : MonoBehaviour
{
    public AudioMixer Main;
    public Slider MasterVolSlider;
    public Slider EffectsVolSlider;
    public Slider MusicVolSlider;
    public static bool closedCaptions;
    public Toggle captionsToggle;
    public TMP_Dropdown QualitySettingsDropdown;
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullscreenToggle;
    Resolution[] m_resolutions;
    Resolution m_currentResolution;
    void Start()
    {
        LoadGraphicsSettings();
        LoadSettings();
        SetDefaultSettings();
    }
    void SetDefaultSettings()
    {
        if (!PlayerPrefs.HasKey("QualityLevel"))
        {
            QualitySettings.SetQualityLevel(5);
            QualitySettingsDropdown.value = 5;
        }
        if (!PlayerPrefs.HasKey("ResIndex"))
        {
            GetResolutions();
            m_currentResolution = m_resolutions[m_resolutions.Length - 1];
            Screen.SetResolution(Screen.width, Screen.height, true);
            FullscreenToggle.isOn = true;
            ResolutionDropdown.value = m_resolutions.Length - 1;
        }
        if (!PlayerPrefs.HasKey("CC"))
        {
            captionsToggle.isOn = true;
            closedCaptions = true;
        }
    }
    public void SetCaptions(bool _captions)
    {
        closedCaptions = _captions;
        PlayerPrefs.SetInt("CC", (_captions ? 1 : 0));
    }
    public void MasterVol(float _level)
    {
        Main.SetFloat("Master", Mathf.Log10(_level) * 20);
        PlayerPrefs.SetFloat("Master", Mathf.Log10(_level) * 20);
    }
    public void EffectsVol(float _level)
    {
        Main.SetFloat("Effects", Mathf.Log10(_level) * 20);
        PlayerPrefs.SetFloat("Effects", Mathf.Log10(_level) * 20);
    }
    public void MusicVol(float _level)
    {
        Main.SetFloat("Music", Mathf.Log10(_level) * 20);
        PlayerPrefs.SetFloat("Music", Mathf.Log10(_level) * 20);
    }
    public void SetFullscreen(bool _state)
    {
        Screen.fullScreen = _state;
        PlayerPrefs.SetInt("Fullscreen", (_state ? 1 : 0));
    }
    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Master"))
        {
            MasterVolSlider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("Master") / 20);
            Main.SetFloat("Effects", Mathf.Log10(MasterVolSlider.value) * 20);
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            MusicVolSlider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("Music") / 20);
            Main.SetFloat("Effects", Mathf.Log10(MusicVolSlider.value) * 20);
        }
        if (PlayerPrefs.HasKey("Effects"))
        {
            EffectsVolSlider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("Effects") / 20);
            Main.SetFloat("Effects", Mathf.Log10(EffectsVolSlider.value) * 20);
        }
    }
    public void GetResolutions()
    {
        m_resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        List<string> ResolutionNames = new List<string>();
        string resolutionName;
        foreach (Resolution res in m_resolutions)
        {
            if (res.width >= 1366 && res.height >= 768)
            {
                resolutionName = "" + res.width + " x " + res.height + " @ " + res.refreshRate + " hz";
                ResolutionNames.Add(resolutionName);
            }
        }
        ResolutionDropdown.AddOptions(ResolutionNames);
    }
    public void SetResolution(int _index)
    {
        Resolution res = m_resolutions[_index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResIndex", _index);
    }
    public void SetQualitySetting(int _index)
    {
        QualitySettings.SetQualityLevel(_index);
        PlayerPrefs.SetInt("QualityLevel", _index);
    }
    public void LoadGraphicsSettings()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));
        QualitySettingsDropdown.value = PlayerPrefs.GetInt("QualityLevel");
        GetResolutions();
        m_currentResolution = m_resolutions[PlayerPrefs.GetInt("ResIndex")];
        Screen.SetResolution(m_currentResolution.width, m_currentResolution.height, true);
        ResolutionDropdown.value = PlayerPrefs.GetInt("ResIndex");
        int boolState = PlayerPrefs.GetInt("Fullscreen");
        if (boolState <= 0)
        {
            Screen.fullScreen = false;
            FullscreenToggle.isOn = false;
        }
        else
        {
            Screen.fullScreen = true;
            FullscreenToggle.isOn = true;
        }
        int boolStateCC = PlayerPrefs.GetInt("CC");
        if (boolStateCC <= 0)
        {
            captionsToggle.isOn = false;
            closedCaptions = false;
        }
        else
        {
            captionsToggle.isOn = true;
            closedCaptions = true;
        }
    }
}
