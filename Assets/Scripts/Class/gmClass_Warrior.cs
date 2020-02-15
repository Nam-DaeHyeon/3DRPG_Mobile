using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gmClass_Warrior : gmClass
{
    public override void Define_ClassData()
    {
        _name = "warrior";

        Spell temp = gameObject.AddComponent<Spell_Rush>();
        _spells.Add(temp);

        temp = gameObject.AddComponent<Spell_HealthBless>();
        _spells.Add(temp);

        temp = gameObject.AddComponent<Spell_Barrier>();
        _spells.Add(temp);
    }
}
