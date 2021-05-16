using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI SoulsLeftText;
    public TextMeshProUGUI MaxStatText;
    public TextMeshProUGUI Info;
    public bool IsDamageMenu;
    ShopSystem m_sSystem;
    private void Start()
    {
        m_sSystem = GameObject.Find("GameManager").GetComponent<ShopSystem>();
        if (IsDamageMenu)
            Info.text = "Spend " + m_sSystem.GetDamageCostUpgrade() + " souls\n" + "To increase your damage by\n" + m_sSystem.GetAmountOfDamage();
        else
            Info.text = "Spend " + m_sSystem.GetSanityCostUpgrade() + " souls\n" + "To increase your sanity by\n" + m_sSystem.GetAmountOfSanity();

    }
    public void DisplaySanityInfo(float _costSanityUpgrade, float _amountOfSanityToIncrease, PlayerController _pController)
    {
        Info.text = "Spend " + _costSanityUpgrade + " souls\n" + "To increase your sanity by\n" + _amountOfSanityToIncrease;
        SoulsLeftText = GameObject.Find("HUD").transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        SoulsLeftText.text = "Souls Left: " + _pController.souls;
        MaxStatText.text = "Max Sanity: " + _pController.maxSanity;
    }
    public void DisplayDamageInfo(float _costDamageUpgrade, float _amountOfDamageToIncrease, PlayerController _pController)
    {
        Info.text = "Spend " + _costDamageUpgrade + " souls\n" + "To increase your damage by\n" + _amountOfDamageToIncrease;
        SoulsLeftText = GameObject.Find("HUD").transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        SoulsLeftText.text = "Souls Left: " + _pController.souls;
        MaxStatText.text = "Damage: " + _pController.transform.GetChild(0).GetChild(1).GetComponent<HurtBox>().damageVal;
    }
}
