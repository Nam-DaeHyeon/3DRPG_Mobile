using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gmClass_Magician : gmClass
{
    public override void Define_ClassData()
    {
        _name = "magician";

        Spell temp = gameObject.AddComponent<Spell_Explosion>();
        _spells.Add(temp);

        temp = gameObject.AddComponent<Spell_Healing>();
        _spells.Add(temp);

        temp = gameObject.AddComponent<Spell_LightningBolt>();
        _spells.Add(temp);
    }

}
