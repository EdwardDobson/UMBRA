using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RoomLightChange : MonoBehaviour
{
    public Light2D Light;
    [SerializeField]
    bool m_startFade;
    [SerializeField]
    bool m_startFadeReturn;
    void Update()
    {
        if(m_startFade)
        {
            LightFade();
            m_startFadeReturn = false;
        }
        if(m_startFadeReturn)
        {
            LightFadeReturn();
            m_startFade = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            m_startFadeReturn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player"))
        {
            m_startFade = true;
        }
    }
    void LightFade()
    {
        Light.intensity += Time.deltaTime * 1;
        if(Light.intensity >= 1)
        {
            m_startFade = false;
            Light.intensity = 1;
        }
    }
    void LightFadeReturn()
    {
        Light.intensity -= Time.deltaTime * 1;
        if (Light.intensity <= 0.2f)
        {
            m_startFadeReturn = false;
            Light.intensity = 0.2f;
        }
    }
}
