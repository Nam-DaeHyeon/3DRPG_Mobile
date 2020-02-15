using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SPELL_TARGETING
{
    SELF,
    MYAREA,
    NON_TARGET,
    TARGET,
}

public enum SPELL_TYPE
{
    MONSTER,
    HEAL,
    BUFF,
    DEBUFF
}

public class Spell : MonoBehaviour
{
    protected Player _player;

    public bool isLearn;

    protected string _name;
    protected int _consumeSP;
    protected float _currUsingTime;
    protected float _maxUsingTime;
    protected float _currCool;
    protected float _maxCool;
    protected int _readyLv;
    protected SPELL_TARGETING _targetingType = SPELL_TARGETING.NON_TARGET;
    protected SPELL_TYPE _affectType = SPELL_TYPE.MONSTER;

    //스킬 계수
    protected float _bonusAdd;          //덧셈계수
    protected float _bonusMulti = 1;    //곱셈계수

    bool _spellOn;
    protected Vector3 _spellPos;
    Transform _castObj;

    protected float _spellRadiusSize;

    private UI_QuickButton _btnEvent;

    [Header("UI")]
    public Sprite spellIconImage;

    public void Set_SpellCaster(Player player)
    {
        _player = player;
    }

    public void Cast(UI_QuickButton btnEvent, Transform castObj)
    {
        if (_spellOn) return;
        _spellOn = true;
        _castObj = castObj;

        _btnEvent = btnEvent;
        _btnEvent.EventTrigger_OnPointerDown();

        StartCoroutine(IE_Spell_Process_1_Targeting());
    }

    IEnumerator IE_Spell_Process_1_Targeting()
    {
        switch (_targetingType)
        {
            case SPELL_TARGETING.NON_TARGET:
                yield return new WaitUntil(() => !_btnEvent.trigger);
                _spellPos = _castObj.transform.position;
                break;
            case SPELL_TARGETING.MYAREA:
                _spellPos = _player.transform.position;
                break;
            case SPELL_TARGETING.SELF:
                _spellPos = _player.transform.position;
                break;
        }

        StartCoroutine(IE_Spell_Process_2_Main());
    }

    IEnumerator IE_Spell_Process_2_Main()
    {
        Spell_Process();

        //애니메이션

        yield return new WaitForSeconds(0.1f);
        Spell_EndTrigger();

        yield break;
    }

    protected virtual void Spell_Process()
    {
        //SE
        SoundManager.instance.Play_SE_Keyword(_name);

        //스킬 사용 효과.
        if(!_targetingType.Equals(SPELL_TARGETING.SELF)) StartCoroutine(IE_TriggerCol());

        //스킬 사용 쿨타임 적용.
        StartCoroutine(IE_CoolTimerON());
    }

    protected void Spell_EndTrigger()
    {
        _spellOn = false;
    }

    IEnumerator IE_CoolTimerON()
    {
        _currCool = _maxCool;

        while(_currCool > 0)
        {
            _currCool -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator IE_TriggerCol()
    {
        _player.Active_SpellCollider(_spellRadiusSize, _spellPos);
        yield return new WaitForSeconds(0.5f);
        _player.Unactive_SpellCollider();

        switch (_affectType)
        {
            case SPELL_TYPE.MONSTER:
                _player.Calculate_Spell_HitMonster(this);
                break;
            case SPELL_TYPE.HEAL:
                _player.Calculate_Spell_HitPlayer(this);
                break;
        }

    }

    public SPELL_TARGETING Get_SPELL_TARGETING()
    {
        return _targetingType;
    }

    public bool Get_SpellON() { return _spellOn; }
    public float Get_CurrentCoolTime() { return _currCool; }
    public float Get_MaxCoolTime() { return _maxCool; }
    public int Get_ConsumeSP() { return _consumeSP; }
    public string Get_SpellName() { return _name; }
    public int Get_ReadyLevel() { return _readyLv; }
    public float Get_BonusMulti() { return _bonusMulti; }
    public float Get_BonusAdd() { return _bonusAdd; }

    public bool Get_Triggered() { return _btnEvent.trigger; }

    public Sprite Get_Sprite_SpellIcon()
    {
        return Resources.Load<Sprite>("Spell/Spell_" + _name);
    }

    public float Get_SpellRadius() { return _spellRadiusSize; }
}
