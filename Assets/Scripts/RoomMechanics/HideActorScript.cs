using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]

public class HideActorScript : MonoBehaviour
{
    [SerializeField]
    DeviceManager m_deviceManager;
    public bool HideActorIndefinitely;
    ParticleSystem m_pSystem;

    //This is a temp script to hide actors until the hide actors script is fixed (in the device manager).
    private void Start()
    {
        m_pSystem = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (HideActorIndefinitely == true)
        {
            if (m_deviceManager.PerformAction)
            {
                Off();
            }
        }
        else
        {
            if (m_deviceManager.PerformAction)
            {
                Off();
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                if (m_pSystem != null && !m_pSystem.isPlaying)
                    m_pSystem.Play();
            }
        }
    }
    void Off()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        if (m_pSystem != null)
            m_pSystem.Stop();
    }
}
