using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gmClass : MonoBehaviour
{
    protected string _name;
    protected List<Spell> _spells = new List<Spell>();

    public virtual void Define_ClassData()
    {

    }
    
    public string Get_ClassName() { return _name; }
    public List<Spell> Get_ClassSpell() { return _spells; }

    private void Awake()
    {
        Define_ClassData();
    }
}
