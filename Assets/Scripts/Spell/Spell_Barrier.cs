using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Barrier : Spell
{
    public Spell_Barrier()
    {
        isLearn = false;

        _name = "Barrier";
        _maxCool = 60;
        _consumeSP = 75;

        _readyLv = 10;

        _bonusMulti = 50;

        _affectType = SPELL_TYPE.BUFF;
        _targetingType = SPELL_TARGETING.SELF;
    }

    protected override void Spell_Process()
    {
        EffectManager.instance.PlayerEffect_Barrier(_player.transform);

        base.Spell_Process();

        StartCoroutine(IE_Buff());
    }

    private IEnumerator IE_Buff()
    {
        _player.Set_Buff_DivideDamage(true);
        yield return new WaitForSeconds(15f);
        _player.Set_Buff_DivideDamage(false);
        
        EffectManager.instance.PlayerEffect_Barrier_OFF();
    }
}
