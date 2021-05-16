using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticle : MonoBehaviour
{
    ParticleSystem m_system;
    ParticleSystem.MainModule m_main;
    public Color StartColour;
    void Start()
    {
        m_system = GetComponent<ParticleSystem>();
        var colOverTime = m_system.colorOverLifetime;
        colOverTime.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(StartColour, 0.0f), new GradientColorKey(StartColour, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        colOverTime.color = grad;
        m_main = m_system.main ;


        Destroy(gameObject, m_main.duration);
    }
}
