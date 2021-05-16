using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    [SerializeField]
    DeviceManager m_deviceManager;
    public SFXScriptable DoorOpen;
    public SFXScriptable DoorClose;
    bool m_playSound;
    void Update()
    {
        if(m_deviceManager != null)
        {
            if (m_deviceManager.PerformAction)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                if (!m_playSound)
                {
                    AudioManager.Instance.PlaySFX(DoorOpen.audioClips[0], DoorOpen.audioCaption);
                    m_playSound = true;
                }
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                if (m_playSound)
                {
                    AudioManager.Instance.PlaySFX(DoorClose.audioClips[0], DoorClose.audioCaption);
                    m_playSound = false;
                }
            }
        }
   
    }
}
