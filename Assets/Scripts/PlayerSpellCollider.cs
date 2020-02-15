using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellCollider : MonoBehaviour
{
    [SerializeField] SphereCollider _sphereCol;

    List<Monster> targets = new List<Monster>();
    List<Player> players = new List<Player>();
    Monster tempMonster;
    Player tempPlayer;

    private void OnEnable()
    {
        targets.Clear();
    }

    public void Set_Sphere_Radius(float radius)
    {
        _sphereCol.radius = radius;
    }

    public List<Monster> Get_Targets() { return targets; }
    public List<Player> Get_Players() { return players; }

    public void Clear_Lists() { targets.Clear(); players.Clear(); }

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

        if(other.GetComponent<Player>())
        {
            tempPlayer = other.GetComponent<Player>();
            if (!players.Contains(tempPlayer)) players.Add(tempPlayer);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Monster>())
        {
            tempMonster = other.GetComponent<Monster>();
            if (targets.Contains(tempMonster))
            {
                targets.Remove(tempMonster);
            }
        }

        if (other.GetComponent<Player>())
        {
            tempPlayer = other.GetComponent<Player>();
            if (players.Contains(tempPlayer)) players.Remove(tempPlayer);
        }
    }
}
