using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeCollider : MonoBehaviour
{
    List<Monster> targets = new List<Monster>();
    Monster tempMonster;

    [SerializeField] SphereCollider _collider;

    private void OnEnable()
    {
        targets.Clear();
    }

    public void Set_Radius(float radius) { _collider.radius = radius; }
    public List<Monster> Get_Targets() { return targets; }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Monster>())
        {
            tempMonster = other.GetComponent<Monster>();
            if (!targets.Contains(tempMonster))
            {
                targets.Add(tempMonster);
            }
        }
    }
}
