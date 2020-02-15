using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Healing : Spell
{
    public Spell_Healing()
    {
        isLearn = false;

        _name = "Healing";
        _maxCool = 5;
        _consumeSP = 55;

        _readyLv = 5;

        _bonusMulti = 10;
        _bonusAdd = 50;

        _spellRadiusSize = 5f;

        _affectType = SPELL_TYPE.HEAL;
        _targetingType = SPELL_TARGETING.MYAREA;
    }

    protected override void Spell_Process()
    {
        EffectManager.instance.PlayerEffect_Healing(_spellPos);

        base.Spell_Process();
    }
}
