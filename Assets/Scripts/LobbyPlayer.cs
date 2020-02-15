using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    public string nickName;
    public string className;
    public int level;
    public int money;
    public int ab_str;
    public int ab_con;
    public int ab_int;
    public int ab_wis;
    public int ab_dex;
    public int point;

    Animator _animator;
    CharacterController _cc;

    Vector3 _originPos;
    Vector3 _originRot;
    Vector3 _targetPos;
    
    Vector3 dir;
    float _speed = 0.7f;
    float _distance;

    private void Start()
    {
        _originPos = transform.position;
        _originRot = transform.eulerAngles;

        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
    }

    public void Move_toTargetPos(Vector3 target)
    {
        _targetPos = new Vector3(target.x, transform.position.y, target.z);
        _distance = Vector3.Distance(_targetPos, transform.position);

        StartCoroutine(IE_Controller());
    }

    public void Move_toOriginPos()
    {
        transform.position = _originPos;
        transform.eulerAngles = _originRot;
    }

    IEnumerator IE_Controller()
    {
        while(_distance > 1.2f && LobbyManager.instance.Get_goPlayer().Equals(this))
        {
            transform.LookAt(_targetPos);

            _animator.SetFloat("MOVE_FV", 1);

            dir = _targetPos - transform.position;
            dir.y = Physics.gravity.y;

            _cc.Move(dir * _speed * Time.deltaTime);

            _targetPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
            _distance = Vector3.Distance(_targetPos, transform.position);

            yield return null;
        }

        _animator.SetFloat("MOVE_FV", 0);
        if(!LobbyManager.instance.Get_goPlayer().Equals(this)) Move_toOriginPos();
    }
}
