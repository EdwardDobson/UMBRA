using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Links all of the shop system displays together
/// </summary>
public class ShopSystem : MonoBehaviour
{
    [SerializeField]
    float m_costOfSanityUpgrade;
    [SerializeField]
    float m_costOfDamageUpgrade;
    [SerializeField]
    int m_amountOfSanityToIncrease;
    [SerializeField]
    int m_amountOfDamageToIncrease;
    [SerializeField]
    float m_sanityCostMultiplier;
    [SerializeField]
    float m_damageCostMultiplier;
    PlayerController m_pController;
    HurtBox m_hBox;
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            m_pController = GameObject.Find("Player").GetComponent<PlayerController>();
            m_hBox = GameObject.Find("Player").transform.GetChild(0).GetChild(1).GetComponent<HurtBox>();
        }
     
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Add(ref float _valueToChange, ref int _addAmount, ref float _cost, ref float _multiplier)
    {
        _valueToChange += _addAmount;
        m_pController.souls -= (int)_cost;
        _addAmount += _addAmount;
        _cost += _cost * _multiplier;
        _cost = (int)_cost;
    }
    public void AddToSanityCap()
    {
        if (m_pController.souls >= m_costOfSanityUpgrade)
        {
            Add(ref m_pController.maxSanity, ref m_amountOfSanityToIncrease, ref m_costOfSanityUpgrade, ref m_sanityCostMultiplier);
        }
    }
    public void AddToDamageCap()
    {
        if (m_pController.souls >= m_costOfDamageUpgrade)
        {
            Add(ref m_hBox.damageVal, ref m_amountOfDamageToIncrease, ref m_costOfDamageUpgrade, ref m_damageCostMultiplier);
        }
    }
    public void ResetTime()
    {
        Time.timeScale = 1;
    }
    public float GetSanityCostUpgrade()
    {
        return m_costOfSanityUpgrade;
    }
    public float GetDamageCostUpgrade()
    {
        return m_costOfDamageUpgrade;
    }
    public float GetAmountOfDamage()
    {
        return m_amountOfDamageToIncrease;
    }
    public float GetAmountOfSanity()
    {
        return m_amountOfSanityToIncrease;
    }
    public PlayerController GetPlayerController()
    {
        return m_pController;
    }
}
