using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject ShopWindow;
    ControlBindings m_cBindings;
    ShopSystem m_sSystem;
    public bool IsDamageMenu;
    PlayerController m_pController;
    private void Start()
    {
        m_cBindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
        m_sSystem = GameObject.Find("GameManager").GetComponent<ShopSystem>();
        m_pController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            ShopWindow.SetActive(true);
            if (IsDamageMenu)
            {
                ShopWindow.GetComponent<Shop>().DisplayDamageInfo(m_sSystem.GetDamageCostUpgrade(), m_sSystem.GetAmountOfDamage(), m_pController);
            }
            if (!IsDamageMenu)
            { 
                ShopWindow.GetComponent<Shop>().DisplaySanityInfo(m_sSystem.GetSanityCostUpgrade(), m_sSystem.GetAmountOfSanity(), m_pController);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            ShopWindow.SetActive(false);
        }
    }
    private void Update()
    {
        if(ShopWindow.activeSelf)
        {
            if (Input.GetKeyDown(m_cBindings.Interact) || Input.GetButtonDown("ButtonA"))
            {
                if (IsDamageMenu)
                {
                    m_sSystem.AddToDamageCap();
                    ShopWindow.GetComponent<Shop>().DisplayDamageInfo(m_sSystem.GetDamageCostUpgrade(),m_sSystem.GetAmountOfDamage(),m_sSystem.GetPlayerController());
                }
                if (!IsDamageMenu)
                {
                    m_sSystem.AddToSanityCap();
                    ShopWindow.GetComponent<Shop>().DisplaySanityInfo(m_sSystem.GetSanityCostUpgrade(),m_sSystem.GetAmountOfSanity(),m_sSystem.GetPlayerController());
                }
            }
        }
    }
}
