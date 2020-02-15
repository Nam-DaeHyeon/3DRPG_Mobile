using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [Header("Player Controller")]
    [SerializeField] Camera _trackingCam;
    [SerializeField] float _speed = 5.0f;
    [SerializeField] float _jumpPower = 10f;
    float _yVelocity;

    [SerializeField] Animator _animator;
    [SerializeField] CharacterController _cc;
    [SerializeField] Joystick _joystick;

    float currv, currh;
    float prevv, prevh;
    Vector3 dir;
    Vector3 lookDir;
    bool isGround;      //바닥에 닿았는가 / 착지했는가
    bool isJump;        //점프 중인가
    bool isAttack;      //공격 중인가
    bool isCast;        //스킬 시전 중인가

    int anim_attkIndex = 0;

    float animTime_attack = 1f;

    float anim_readyTime = 3f;

    IEnumerator IE_Controller()
    {
        while (p_State.Equals(PlayerState.NONE))
        {
            Move();
            GroundCheck();
            yield return null;
        }
    }
    
    private void Move()
    {
        if (p_State.Equals(PlayerState.DEAD)) return;
        if (isAttack) return;

        prevv = currv;
        prevh = currh;

        //currv = Input.GetAxis("Vertical");
        //currh = Input.GetAxis("Horizontal");
        currv = _joystick.Vertical;
        currh = _joystick.Horizontal;

        //이동 방향 지정
        dir = Vector3.forward * currv + Vector3.right * currh;

        //시선 좌표 설정
        if (!isCast)
        {
            lookDir = dir + transform.position;
            transform.LookAt(new Vector3(lookDir.x, transform.position.y, lookDir.z));
        }

        //속도 적용
        dir *= _speed;

        //애니메이션 적용
        if (!isCast)
        {
            _animator.SetFloat("MOVE_FV", Mathf.Abs(currv));
            _animator.SetFloat("MOVE_FH", Mathf.Abs(currh));
        }
        
        {
            dir.y = _yVelocity;
            _yVelocity += Physics.gravity.y * Time.deltaTime;
        }

        //캐릭터 이동 처리
        if (!isCast) _cc.Move(dir * Time.deltaTime);
        else if(isCast && !isJump) _castObj.transform.Translate(new Vector3(dir.x, 0, dir.z) * Time.deltaTime);
    }

    private void Jump()
    {
        _yVelocity = _jumpPower;
        isGround = false;
        isJump = true;
        _animator.SetTrigger("JUMP");

        //공격 취소
        if (isAttack)
        {
            StopCoroutine("IE_Attack_Melee");
            isAttack = false;
        }
    }

    private void GroundCheck()
    {
        if (_cc.isGrounded)
        {
            if (isGround) return;

            if (isJump)
            {
                _animator.SetTrigger("LAND");
                isJump = false;
            }

            isGround = true;
        }
        //else isGround = false;
    }

    IEnumerator IE_Attack_Melee()
    {
        if (isAttack) yield break;
        isAttack = true;

        _animator.SetBool("READY_TOATTACK", true);
        
        StopCoroutine("IE_ReadyToIdleTimer");
        StartCoroutine("IE_ReadyToIdleTimer");

        float temp = animTime_attack * 0.34f;
        
        switch(anim_attkIndex)
        {
            case 0:
                _animator.SetTrigger("ATTACK_LEFT");
                break;
            case 1:
                _animator.SetTrigger("ATTACK_RIGHT");
                break;
        }
        anim_attkIndex++;
        if (anim_attkIndex >= 2) anim_attkIndex = 0;

        yield return new WaitForSeconds(temp - 0.1f);

        //유효타
        _meleeCol.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _meleeCol.gameObject.SetActive(false);

        Monster target = null;
        Vector3 monsterPos;
        for(int i = 0; i < _meleeCol.Get_Targets().Count; i++)
        {
            target = _meleeCol.Get_Targets()[i];

            //공격범위에 있는가
            
            monsterPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

            //탱크로부터 타겟까지의 단위벡터
            Vector3 dirToTarget = (monsterPos - transform.position).normalized;

            // transform.forward와 dirToTarget은 모두 단위벡터이므로 내적값은 두 벡터가 이루는 각의 Cos값과 같다.
            // 내적값이 시야각/2의 Cos값보다 크면 시야에 들어온 것이다.
            if (Vector3.Dot(transform.forward, dirToTarget) > Mathf.Cos((_attackAngle / 2) * Mathf.Deg2Rad))
            //if (Vector3.Angle(_transform.forward, dirToTarget) < ViewAngle/2)
            {
                target.TakeDamage(_attack);
            }
            else continue;

            //몬스터를 죽였습니다.
            if (target.Get_CurrentHP() <= 0)
            {
                Get_Exp_Money();
            }
        }

        yield return new WaitForSeconds(animTime_attack - temp);

        isAttack = false;
    }

    private void Get_Exp_Money()
    {
        StartCoroutine(IE_Get_Exp_Money());
    }

    private IEnumerator IE_Get_Exp_Money()
    {
        //경험치 획득 = 레벨업
        GameManager.instance.playerData.Add_LEVEL();
        GameManager.instance.playerData.Add_POINT();
        HUDMessageManager.instance.Load_TextMessage(transform.position, "LEVEL UP");
        //재화 획득 INT(5)
        GameManager.instance.playerData.Add_Money(Random.Range(10, 50));

        //서버 동기화
        MySQLManager.instance.Update_PlayingCharacterData();
        yield return new WaitUntil(() => MySQLManager.instance.Get_EndCheck());
    }

    private IEnumerator IE_ReadyToIdleTimer()
    {
        yield return new WaitForSeconds(anim_readyTime);

        _animator.SetBool("READY_TOATTACK", false);
        _animator.SetTrigger("IDLE");
    }

    public void UI_Button_Attack()
    {
        if (p_State.Equals(PlayerState.DEAD)) return;
        if (!isGround || isCast) return;

        StartCoroutine("IE_Attack_Melee");
    }

    public void UI_Button_Jump()
    {
        Jump();
    }
}
