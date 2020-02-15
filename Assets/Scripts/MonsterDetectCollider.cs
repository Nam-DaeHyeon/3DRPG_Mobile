using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetectCollider : MonoBehaviour
{
    [SerializeField] SphereCollider _collder;
    Monster _owner;
    Player _targetPlayer;

    public void Set_SphereColliderRadius(float radius)
    {
        _collder.radius = radius;
    }

    public void Set_ColliderOwner(Monster owner)
    {
        _owner = owner;
    }

    public void Enable_Collider() { _collder.enabled = true; }
    public void Disable_Collider() { _collder.enabled = false; }

    public Player Get_TargetPlayer() { return _targetPlayer; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
        {
            _targetPlayer = other.GetComponent<Player>();

            _owner.Set_TargetPlayer(_targetPlayer);
            _owner.Set_StateMachine(Monster.MonsterState.MOVE);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Player>().Equals(_targetPlayer))
        {
            _targetPlayer = null;

            _owner.Play_AnimatorTrigger("IDLE");
            _owner.Set_StateMachine(Monster.MonsterState.IDLE);
        }
    }
}
