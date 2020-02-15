using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_HealthBless : Spell
{
    public Spell_HealthBless()
    {
        isLearn = false;

        _name = "HealthBless";
        _maxCool = 30;
        _consumeSP = 50;
        
        _readyLv = 3;

        _bonusMulti = 5;

        _affectType = SPELL_TYPE.BUFF;
        _targetingType = SPELL_TARGETING.SELF;
    }

    protected override void Spell_Process()
    {
        EffectManager.instance.PlayerEffect_HealthBless(_player.transform);

        base.Spell_Process();

        StartCoroutine(IE_Buff());
    }

    private IEnumerator IE_Buff()
    {
        float healAmount = _player.Get_MaxHP() * 0.01f * _bonusMulti;
        for (float t = 10; t > 0; t--)
        {
            yield return new WaitForSeconds(1);
            _player.TakeHeal(healAmount);
        }

        EffectManager.instance.PlayerEffect_HealthBless_OFF();
    }
}
