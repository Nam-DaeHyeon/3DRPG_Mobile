using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Rush : Spell
{
    public Spell_Rush()
    { 
        isLearn = true;

        _name = "Rush";
        _maxCool = 10;
        _consumeSP = 20;

        _maxUsingTime = 5f;

        _bonusMulti = 0.3f;

        _spellRadiusSize = 4f;

        _affectType = SPELL_TYPE.MONSTER;
        _targetingType = SPELL_TARGETING.SELF;
    }

    protected override void Spell_Process()
    {
        EffectManager.instance.PlayerEffect_Rush(_player.transform);

        base.Spell_Process();

        StartCoroutine(IE_Rush());
    }

    private IEnumerator IE_Rush()
    {
        _currUsingTime = _maxUsingTime;
        _player.Active_SpellCollider(_spellRadiusSize, _spellPos);

        while (_currUsingTime >= 0)
        {
            yield return new WaitForSeconds(0.8f);
            _currUsingTime -= 0.8f;

            switch (_affectType)
            {
                case SPELL_TYPE.MONSTER:
                    _player.Calculate_Spell_HitMonster(this);
                    break;
            }
            yield return null;
        }

        EffectManager.instance.PlayerEffect_Rush_OFF();
        _player.Unactive_SpellCollider();
    }
}
