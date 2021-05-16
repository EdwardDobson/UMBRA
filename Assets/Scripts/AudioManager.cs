using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;

    public AudioMixer MasterMixer;
    public AudioMixerGroup MusicMixerGroup;
    public AudioMixerGroup SFXMixerGroup;

    //Music
    [SerializeField]
    AudioClip musicClip;
    AudioSource MusicSource;

    //filters
    AudioLowPassFilter lowPass;
    AudioDistortionFilter distortFilter;
    AudioChorusFilter chorusFilter;

    //SFX
    public List<AudioSource> sfxList = new List<AudioSource>();

    private void Awake()
    {
        //Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MusicSource = gameObject.AddComponent<AudioSource>();
        MusicSource.outputAudioMixerGroup = MusicMixerGroup;

        MusicSource.clip = musicClip;
        MusicSource.loop = true;

        lowPass = gameObject.AddComponent<AudioLowPassFilter>();
        distortFilter = gameObject.AddComponent<AudioDistortionFilter>();
        chorusFilter = gameObject.AddComponent<AudioChorusFilter>();

        lowPass.cutoffFrequency = 3000;
        lowPass.lowpassResonanceQ = 1;

        distortFilter.distortionLevel = 0.2f;

        chorusFilter.dryMix = 0.7f;
        chorusFilter.wetMix1 = 0.5f;
        chorusFilter.wetMix2 = 0.5f;
        chorusFilter.wetMix3 = 0.5f;
        chorusFilter.delay = 50f;
        chorusFilter.rate = 0.4f;
        chorusFilter.depth = 0.2f;

        lowPass.enabled = false;
        distortFilter.enabled = false;
        chorusFilter.enabled = false;
    }

    public void FiltersOn()
    {
        lowPass.enabled = true;
        distortFilter.enabled = true;
        chorusFilter.enabled = true;
    }

    public void FiltersOff()
    {
        lowPass.enabled = false;
        distortFilter.enabled = false;
        chorusFilter.enabled = false;
    }

    public void StopMusic()
    {
        if (MusicSource != null)
        {
            if (MusicSource.isPlaying)
            {
                MusicSource.Stop();
            }
        }
    
    }

    public void StartMusic()
    {
        if (MusicSource != null)
        {
            if (!MusicSource.isPlaying)
            {
                MusicSource.Play();
            }
        }
    }

    public void RestartMusic()
    {
        FiltersOff();
        if (MusicSource.isPlaying)
        {
            MusicSource.Stop();
            MusicSource.Play();
        }
    }

    public void Distort()
    {
        
    }


    private void Update()
    {
        //Delete SFX objects that are no longer playing
        for (int i = 0; i < sfxList.Count; i++)
        {
            if (sfxList[i] != null)
            {
                if (!sfxList[i].isPlaying)
                {
                    AudioSource reference = sfxList[i];
                    sfxList.Remove(reference);
                    Destroy(reference.gameObject);
                }
            }
        }
    }

    //Creates and SFX object and plays it
    public void PlaySFX(AudioClip _sfxClip, string _caption)
    {
        GameObject sfxChild = new GameObject();
        AudioSource sfx = sfxChild.AddComponent<AudioSource>();
        sfx.outputAudioMixerGroup = SFXMixerGroup;
        sfx.clip = _sfxClip;
        sfx.Play();
        sfxList.Add(sfx);
        if (Settings.closedCaptions)
        {
            ClosedCaptions.SpawnCaption(_caption);
        }
    }
    public void PlaySFX(AudioClip _sfxClip)
    {
        GameObject sfxChild = new GameObject();
        AudioSource sfx = sfxChild.AddComponent<AudioSource>();
        sfx.outputAudioMixerGroup = SFXMixerGroup;
        sfx.clip = _sfxClip;
        sfx.Play();
        sfxList.Add(sfx);
    }
}
