using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickup : MonoBehaviour
{
    public SFXScriptable pickupSound;
    public int Value;
    public Side PickupSide;
    GameObject m_player;
    PlayerController m_pController;
    SwitchForm m_switchForm;
    private void Start()
    {
        m_player = GameObject.Find("Player");
        m_switchForm = m_player.GetComponent<SwitchForm>();
        m_pController = m_player.GetComponent<PlayerController>();
    }
    private void Update()
    {
        if(PickupSide == Side.Ghost && !m_switchForm.IsInGhostForm())
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (PickupSide == Side.Ghost && m_switchForm.IsInGhostForm())
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
        if (PickupSide == Side.Player && m_switchForm.IsInGhostForm())
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (PickupSide == Side.Player && !m_switchForm.IsInGhostForm())
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
        if(PickupSide == Side.Both)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player"))
        {
            AudioManager.Instance.PlaySFX(pickupSound.audioClips[0], pickupSound.audioCaption);
            m_pController.souls += Value;
            Destroy(gameObject);
        }
    }
}
