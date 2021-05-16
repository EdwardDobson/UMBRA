using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGPUI;
public class TipPlacement : MonoBehaviour
{
    [SerializeField]
    Vector2 m_placePos;
    [SerializeField]
    string m_tipInformation;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag.Contains("Player"))
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(m_placePos);
            pos = new Vector3(pos.x, pos.y, pos.z);
            DialogBoxes.DisplayTip(pos, m_tipInformation);
        }
    }
}
