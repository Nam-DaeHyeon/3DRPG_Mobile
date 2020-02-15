using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum MonsterState
    {
        IDLE,
        MOVE,
        ATTACK,
        HIT,
        DEAD,
        RETURN
    }

    [SerializeField] float _currHp;
    [SerializeField] float _maxHp;
    [SerializeField] float _attack;

    [SerializeField] float _moveSpeed = 1.4f;
    [SerializeField] float _chaseDist;
    [SerializeField] float _attackDist;
    [SerializeField] float _regenTime;

    [SerializeField] MonsterState m_State;

    [Header("Component")]
    [SerializeField] MonsterDetectCollider dt_Col;
    [SerializeField] Animator _animator;
    CharacterController _cc;

    Player _targetPlayer;

    private Vector3 _moveDir;
    private float _distance;
    private float yVelocity;

    private float animTime_attack = 0.857f;
    private float animTime_hit = 0.45f;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        Set_Init();
    }

    private void Set_Init()
    {
        _currHp = _maxHp;

        if (StageManager.instance != null)
        {
            transform.position = StageManager.instance.monsterSpawnPos.position;
            transform.rotation = StageManager.instance.monsterSpawnPos.rotation;
        }

        _targetPlayer = null;

        dt_Col.Enable_Collider();
        dt_Col.Set_ColliderOwner(this);
        dt_Col.Set_SphereColliderRadius(_chaseDist);

        _animator.gameObject.SetActive(true);

        Set_StateMachine(MonsterState.IDLE);
    }
    
    public void Set_StateMachine(MonsterState nextState)
    {
        if (!m_State.Equals(MonsterState.IDLE) && nextState.Equals(MonsterState.IDLE)) _animator.SetTrigger("IDLE");
        m_State = nextState;

        _moveDir = new Vector3(0, _moveDir.y, 0);

        StopAllCoroutines();
        StartCoroutine(IE_Gravity());

        switch (m_State)
        {
            case MonsterState.IDLE:
                StartCoroutine(IE_Idle());
                break;
            case MonsterState.MOVE:
                StartCoroutine(IE_MoveTo_TargetPlayer());
                break;
            case MonsterState.ATTACK:
                StartCoroutine(IE_AttackTo_TargetPlayer());
                break;
            case MonsterState.HIT:
                StartCoroutine(IE_HitFrom_TargetPlayer());
                break;
            case MonsterState.DEAD:
                StartCoroutine(IE_RegenTimer());
                break;
            case MonsterState.RETURN:
                StartCoroutine(IE_ReturnTo_Spawner());
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (m_State.Equals(MonsterState.DEAD)) return;

        _currHp -= damage;
        HUDMessageManager.instance.Load_TextMessage(transform.position, damage.ToString());

        if (_currHp <= 0)
        {
            Set_StateMachine(MonsterState.DEAD);
        }
        else
        {
            Set_StateMachine(MonsterState.HIT);
        }
    }

    public void Set_TargetPlayer(Player player) { _targetPlayer = player; }
    public Player Get_TargetPlayer() { return _targetPlayer; }

    IEnumerator IE_Gravity()
    {
        while(true)
        {
            _moveDir.y = yVelocity;
            yVelocity += Physics.gravity.y * Time.deltaTime;

            if (_cc.isGrounded) yVelocity = 0;

            yield return null;
        }
    }

    IEnumerator IE_Idle()
    {
        while(m_State.Equals(MonsterState.IDLE))
        {
            _cc.Move(_moveDir * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator IE_MoveTo_TargetPlayer()
    {
        _animator.SetTrigger("MOVE");

        while(m_State.Equals(MonsterState.MOVE))
        {
            _distance = Vector3.Distance(transform.position, _targetPlayer.transform.position);
            _moveDir = _targetPlayer.transform.position - transform.position;
            _moveDir = new Vector3(_moveDir.x, yVelocity, _moveDir.z);

            transform.LookAt(new Vector3(_targetPlayer.transform.position.x, transform.position.y, _targetPlayer.transform.position.z));

            _cc.Move(new Vector3(_moveDir.x * _moveSpeed, _moveDir.y, _moveDir.z * _moveSpeed) * Time.deltaTime);
            if (_distance <= _attackDist) Set_StateMachine(MonsterState.ATTACK);

            yield return null;
        }
    }

    IEnumerator IE_AttackTo_TargetPlayer()
    {
        _animator.SetTrigger("ATTACK");

        float temp = animTime_attack;
        yield return new WaitForSeconds(temp);

        //유효타
        _distance = Vector3.Distance(transform.position, _targetPlayer.transform.position);
        if(_distance <= _attackDist)
        {
            _targetPlayer.TakeDamage(_attack);
        }

        yield return new WaitForSeconds(animTime_attack - temp);

        //대상이 죽었다면 행동 대기
        if (_targetPlayer.Get_CurrentHP() <= 0)
        {
            _targetPlayer = null;
            Set_StateMachine(MonsterState.RETURN);
            yield break;
        }

        //다음 상태
        _distance = Vector3.Distance(transform.position, _targetPlayer.transform.position);
        if (_distance <= _chaseDist)
        {
            if (_distance <= _attackDist) Set_StateMachine(MonsterState.ATTACK);
            else Set_StateMachine(MonsterState.MOVE);
        }
        else Set_StateMachine(MonsterState.IDLE);

    }

    IEnumerator IE_HitFrom_TargetPlayer()
    {
        _animator.SetTrigger("HIT");

        yield return new WaitForSeconds(animTime_hit);

        _distance = Vector3.Distance(transform.position, _targetPlayer.transform.position);
        if (_distance <= _chaseDist)
        {
            if (_distance <= _attackDist) Set_StateMachine(MonsterState.ATTACK);
            else Set_StateMachine(MonsterState.MOVE);
        }
        else Set_StateMachine(MonsterState.IDLE);
    }

    IEnumerator IE_RegenTimer()
    {
        EffectManager.instance.PlayEffect_WindCutter(transform.position);
        _animator.gameObject.SetActive(false);
        dt_Col.Disable_Collider();

        float leftTime = _regenTime;

        while (m_State.Equals(MonsterState.DEAD))
        {
            if (leftTime <= 0) break;
            leftTime -= Time.deltaTime;

            yield return null;
        }

        for(int i = 0; i < _animator.parameterCount; i++)
        {
            _animator.ResetTrigger(_animator.parameters[i].name);
        }

        Set_Init();
    }

    public void Play_AnimatorTrigger(string key)
    {
        _animator.SetTrigger(key);
    }

    private IEnumerator IE_ReturnTo_Spawner()
    {
        _animator.SetTrigger("MOVE");

        Vector3 pos = StageManager.instance.monsterSpawnPos.position;

        while (m_State.Equals(MonsterState.RETURN))
        {
            _distance = Vector3.Distance(transform.position, pos);
            _moveDir = pos - transform.position;
            _moveDir = new Vector3(_moveDir.x, yVelocity, _moveDir.z);

            transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));

            _cc.Move(new Vector3(_moveDir.x * _moveSpeed, _moveDir.y, _moveDir.z * _moveSpeed) * Time.deltaTime);
            if (_distance <= 0.2f) Set_StateMachine(MonsterState.IDLE);

            yield return null;
        }
    }

    public float Get_CurrentHP()
    {
        return _currHp;
    }
}
