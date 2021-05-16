using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is called in the playerUI script. Reduces the find calls.
/// </summary>

public class DamagePopup : MonoBehaviour
{
     Transform m_parent;
     TextMeshProUGUI m_text;
     float m_duration;
    public void DamagePopSetUp()
    {
        m_parent = GameObject.Find("HUD").transform;
        m_text = GetComponent<TextMeshProUGUI>();
        m_duration = GetComponent<Animation>().clip.length;
    }
    public Color TextColour;
    public void Spawn(Vector2 _startPos, float _value,float _duration)
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(_startPos);
        pos = new Vector3(pos.x, pos.y + 150f);
        GameObject  temp = Instantiate(gameObject, pos, Quaternion.identity, m_parent);
        m_text.text = _value.ToString();
        Destroy(temp, m_duration);
    }
}
