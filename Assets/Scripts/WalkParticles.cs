using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkParticles : MonoBehaviour
{
    ParticleSystem m_system;
    ParticleSystem.MainModule m_main;
    void Start()
    {
        m_system = GetComponent<ParticleSystem>();
        m_main = m_system.main;


        Destroy(gameObject, m_main.duration);
    }
}
