using AGPUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Side
{
    Player,
    Ghost,
    Both,
}
/// <summary>
/// Stores all of the data that each trap share
/// </summary>
public class TrapBaseClass : PossessedObject
{
    [SerializeField]
    protected  Side FormToHit;
    [SerializeField]
    protected bool DeployTrap;
    protected bool FireTrap;
    [SerializeField]
    protected float MaxCoolDown;
    [SerializeField]
    protected float CurrentCoolDown;
    [SerializeField]
    protected bool JustBeATrap;
    [SerializeField]
    protected SwitchForm SwitchForm;
    [SerializeField]
    protected DeviceManager DeviceManager;
    [SerializeField]
    protected float TrapDamage;
    [SerializeField]
    protected float SpeedModifyAmount;
    [SerializeField]
    protected float SpeedDebuffDuration;
    protected BuffManager BuffManager;
    [SerializeField]
    protected bool HasEffect;
    public float KnockbackAmount;
    protected void TrapSetUp()
    {
        BuffManager = GameObject.Find("Player").GetComponent<BuffManager>();
    }
    protected void BaseLoop()
    {
        PerformAction();
        Timer();
        if(IsPossessed && HasAnotherAction)
            DialogBoxes.DisplayButtonPrompt(transform.position, 150, m_cBindings.Interact.ToString());
    }
    protected void Timer()
    {
        if(!JustBeATrap)
        {
            if(DeviceManager != null)
            {
                if (DeployTrap || DeviceManager.PerformAction || IsPossessed && ShouldPerformAction)
                {
                    CurrentCoolDown -= Time.deltaTime;
                    if (CurrentCoolDown <= 0)
                    {
                        FireTrap = true;
                        CurrentCoolDown = MaxCoolDown;
                    }
                }
            }
        }
        else
        {
            if (DeployTrap || IsPossessed && ShouldPerformAction)
            {
                CurrentCoolDown -= Time.deltaTime;
                if (CurrentCoolDown <= 0)
                {
                    FireTrap = true;
                    CurrentCoolDown = MaxCoolDown;
                }
            }
        }
    }
    public void SetDeployTrap(bool _state)
    {
        DeployTrap = _state;
    }
    public Side GetSide()
    {
        return FormToHit;
    }
    public bool GetJustTrap()
    {
        return JustBeATrap;
    }
    public SwitchForm GetSwitchForm()
    {
        return SwitchForm;
    }
    public void SetCurrentCoolDown(float _value)
    {
        CurrentCoolDown = _value;
    }
    public float GetMaxCoolDown()
    {
        return MaxCoolDown;
    }
}

