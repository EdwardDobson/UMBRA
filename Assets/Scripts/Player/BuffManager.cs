using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EffectType
{
    Debuff,
    Buff,
}
public enum EffectAttribute
{
    Damage,
    Speed
}
public class Effect
{
    public float IntervalDurationMax;
    public float IntervalDurationCurrent;
    public float EffectAmount;
    public float KnockbackAmount;
    public float Intervals;
    public EffectType TypeOfBuff;
    public EffectAttribute TypeOfEffect;
    public bool SetEffect;
}
public class BuffManager : MonoBehaviour
{
    public List<Effect> Debuffs = new List<Effect>();
    public List<Effect> Buffs = new List<Effect>();
    private void Update()
    {
        for(int i  =0; i < Debuffs.Count; ++i)
        {
            RunEffect(Debuffs[i]);
        }
        for (int i = 0; i < Buffs.Count; ++i)
        {
            RunEffect(Buffs[i]);
        }
    }
    public void RunEffect(Effect _effectToRun)
    {
        _effectToRun.IntervalDurationCurrent -= Time.deltaTime;
        if (_effectToRun.IntervalDurationCurrent <= 0)
        {
            _effectToRun.Intervals--;
            if (!_effectToRun.SetEffect)
            {
                if(_effectToRun.TypeOfEffect == EffectAttribute.Damage)
                {
                    GetComponent<RecieveDamage>().TakeDamage(_effectToRun.EffectAmount,transform.position, _effectToRun.KnockbackAmount);
                    _effectToRun.IntervalDurationCurrent = _effectToRun.IntervalDurationMax;
                }
                if (_effectToRun.TypeOfEffect == EffectAttribute.Speed)
                {
                    GetComponent<PlayerController>().SetMoveSpeed(_effectToRun.EffectAmount);
                    _effectToRun.IntervalDurationCurrent = _effectToRun.IntervalDurationMax;
                }
            }
            else
            {
                if (_effectToRun.TypeOfEffect == EffectAttribute.Speed)
                {
                    GetComponent<PlayerController>().SetMoveSpeed(_effectToRun.EffectAmount);
                    _effectToRun.IntervalDurationCurrent = _effectToRun.IntervalDurationMax;
                }
            }
        }
        if(_effectToRun.Intervals <= 0)
        {
            if (_effectToRun.TypeOfBuff == EffectType.Debuff)
            {
                if(_effectToRun.TypeOfEffect == EffectAttribute.Speed)
                {
                    GetComponent<PlayerController>().SetMoveSpeed(3);
                }
                Debuffs.Remove(_effectToRun);
            }
            if (_effectToRun.TypeOfBuff == EffectType.Buff)
            {
                Buffs.Remove(_effectToRun);
            }
        }
    }
    /// <summary>
    /// Intervals need to be two or more for overtime effect
    /// </summary>
    /// <param name="_intervalDuration"></param>
    /// <param name="_amount"></param>
    /// <param name="_intervals"></param>
    /// <param name="_setEffect"></param>
    /// <param name="_type"></param>
    /// <param name="_effectType"></param>
    public void AddEffect(float _intervalDuration, float _amount, float _intervals, bool _setEffect, EffectType _type,  EffectAttribute _effectType)
    {
        if(_type == EffectType.Debuff)
        {
            Debuffs.Add(MakeEffect(_intervalDuration, _amount, _intervals, _setEffect,_type, _effectType));
        }
        if (_type == EffectType.Buff)
        {
            Buffs.Add(MakeEffect(_intervalDuration, _amount, _intervals, _setEffect, _type, _effectType));
        }
    }
    public Effect MakeEffect(float _durationMax,float _amount, float _intervals, bool _setEffect, EffectType _type, EffectAttribute _effectType)
    {
        Effect newEffect = new Effect();
        newEffect.EffectAmount = _amount;
        newEffect.IntervalDurationMax = _durationMax;
        newEffect.TypeOfBuff = _type;
        newEffect.Intervals = _intervals;
        newEffect.SetEffect = _setEffect;
        newEffect.TypeOfEffect = _effectType;
        return newEffect;
    }
}
