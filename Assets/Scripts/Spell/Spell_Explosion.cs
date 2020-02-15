using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Explosion : Spell
{
    public Spell_Explosion()
    {
        isLearn = true;

        _name = "Explosion";
        _maxCool = 3;
        _consumeSP = 30;

        _spellRadiusSize = 1.5f;
    }

    protected override void Spell_Process()
    {
        EffectManager.instance.PlayEffect_Explosion(_spellPos);

        base.Spell_Process();
    }
}
