using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu_Spell : MonoBehaviour
{
    [SerializeField] Player _player;
    
    List<Spell> _classSpells = new List<Spell>();

    [SerializeField] List<UI_Menu_Spell_ListElement> _spellList;

    bool isSet = false;

    private void OnEnable()
    {
        Set_InitSpellList();
    }

    private void Set_InitSpellList()
    {
        if (isSet) return;
        _classSpells = _player.Get_ClassSpell();

        for (int i = 0; i < _spellList.Count; i++)
        {
            if(i >= _classSpells.Count)
            {
                _spellList[i].gameObject.SetActive(false);
                continue;
            }

            //아이콘
            _spellList[i].spellIconImage.sprite = _classSpells[i].Get_Sprite_SpellIcon();
            //이름
            _spellList[i].spellNameText.text = _classSpells[i].Get_SpellName();
            //조건
            _spellList[i].spellConditionText.text = "";
            if (_classSpells[i].Get_ReadyLevel() > GameManager.instance.playerData.level) _spellList[i].spellConditionText.text += ("Lv." + _classSpells[i].Get_ReadyLevel());
            //재사용대기시간
            _spellList[i].spellCoolTimeText.text = _classSpells[i].Get_MaxCoolTime().ToString();
            //소모 SP
            _spellList[i].spellSPText.text = _classSpells[i].Get_ConsumeSP().ToString();
            //습득버튼
            if (_classSpells[i].isLearn || _classSpells[i].Get_ReadyLevel() <= GameManager.instance.playerData.level)
            {
                _spellList[i].spellQuickResistButton.gameObject.SetActive(true);
                _spellList[i].spellLearnButton.gameObject.SetActive(false);
            }
            else
            {
                _spellList[i].spellQuickResistButton.gameObject.SetActive(false);

                if (!Check_Conditions(_classSpells[i])) _spellList[i].spellLearnButton.gameObject.SetActive(false);
                else _spellList[i].spellLearnButton.gameObject.SetActive(true);
            }
        }

        isSet = true;
    }

    private bool Check_Conditions(Spell targetSpell)
    {
        //레벨 조건 확인
        if (GameManager.instance.playerData.level >= targetSpell.Get_ReadyLevel()) return true;

        return false;
    }

    public void UI_QuickRegist_Spell(int btnIdx)
    {
        //if (!_classSpells[btnIdx].isLearn) return;

        if (_player.registSpellTemp == null)
        {
            _player.registSpellTemp = _classSpells[btnIdx];

            _player.Set_Registing_Quicks();
        }
        else
        {
            _player.registSpellTemp = null;

            _player.Set_Registed_Quicks();
        }

    }

    public void UI_Learn_Spell(int btnIdx)
    {
        _classSpells[btnIdx].isLearn = true;

        _spellList[btnIdx].spellLearnButton.gameObject.SetActive(false);
        _spellList[btnIdx].spellQuickResistButton.gameObject.SetActive(true);

        _spellList[btnIdx].spellConditionText.text = "";
    }

}
