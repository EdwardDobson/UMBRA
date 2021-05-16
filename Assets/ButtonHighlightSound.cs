using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip menuBlipNoise;

   public  void OnPointerEnter(PointerEventData _data)
    {

        AudioManager.Instance.PlaySFX(menuBlipNoise);
    }
}
