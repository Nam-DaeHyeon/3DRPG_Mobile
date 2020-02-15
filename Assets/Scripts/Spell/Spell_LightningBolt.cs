using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_LightningBolt : Spell
{
    public Spell_LightningBolt()
    {
        isLearn = false;

        _name = "LightningBolt";
        _maxCool = 45;
        _consumeSP = 100;

        _readyLv = 15;

        _bonusMulti = 3.5f;

        _spellRadiusSize = 3.2f;
    }

    protected override void Spell_Process()
    {
        EffectManager.instance.PlayerEffect_LightningBolt(_spellPos);

        base.Spell_Process();
    }
}
