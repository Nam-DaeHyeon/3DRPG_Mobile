using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuickButton : MonoBehaviour
{
    [SerializeField] Player _player;

    public Image quickIconImage;
    public Image quickTimer;
    public Text quickItemCount;
    public Transform quickLinkedTr;

    Spell tempSpell;
    InventoryItemSet tempItemSet;

    //Pointer Trigger
    [HideInInspector] public bool trigger;

    public void Start_Timer(Spell spell)
    {
        tempSpell = spell;
        StartCoroutine(IE_Start_Timer());
    }

    public void Start_Timer(InventoryItemSet itemSet)
    {
        tempItemSet = itemSet;
        StartCoroutine(IE_Start_Timer());
    }

    public void Stop_Timer()
    {
        tempSpell = null;
        tempItemSet = null;
    }

    public void Set_QuickElement()
    {
        if (_player.registItemSetTemp == null && _player.registSpellTemp == null) return;

        if (_player.registSpellTemp != null)
        {
            //이전 등록된 요소 삭제.
            Stop_Timer();

            Spell temp = _player.registSpellTemp;
                        
            quickIconImage.sprite = temp.Get_Sprite_SpellIcon();
            quickIconImage.enabled = true;

            if (temp.Get_CurrentCoolTime() <= 0) quickTimer.fillAmount = 0;
            else quickTimer.fillAmount = temp.Get_CurrentCoolTime() / temp.Get_MaxCoolTime();

            quickItemCount.text = "";

            Start_Timer(temp);

            quickLinkedTr = temp.transform;
            _player.Set_Registed_Quicks();

            _player.registSpellTemp = null;

            return;
        }
        else if(_player.registItemSetTemp != null)
        {
            //이전 등록된 요소 삭제
            Stop_Timer();

            InventoryItemSet temp = _player.registItemSetTemp;

            quickIconImage.sprite = temp.item.Get_Sprite_ItemIcon();
            quickIconImage.enabled = true;

            //아이템 쿨타임 초기화
            if (temp.Get_CurrentCoolTime() <= 0) quickTimer.fillAmount = 0;
            else quickTimer.fillAmount = temp.Get_CurrentCoolTime() / temp.Get_MaxCoolTime();

            quickItemCount.text = temp.itemCount.ToString();

            Start_Timer(temp);

            //quickLinkedTr = temp.transform;
            _player.Set_Registed_Quicks();

            _player.registItemSetTemp = null;

            return;
        }

        quickIconImage.sprite = null;
        quickLinkedTr = null;
    }

    IEnumerator IE_Start_Timer()
    {
        while(tempSpell != null || tempItemSet != null)
        {
            if (tempSpell != null)
            {
                quickTimer.fillAmount = tempSpell.Get_CurrentCoolTime() / tempSpell.Get_MaxCoolTime();
                if (tempSpell.Get_CurrentCoolTime() <= 0) quickTimer.fillAmount = 0;
            }
            else if(tempItemSet != null)
            { 
                //아이템 쿨타임 초기화
                //quickTimer.text = ((int)tempItem.Get_CurrentCoolTime()).ToString();
                //if (tempItem.Get_CurrentCoolTime() <= 0) quickTimer.text = "";
            }
            yield return null;
        }
    }

    public InventoryItemSet Get_RegisteredItemSet() { return tempItemSet; }
    public Spell Get_RegisteredSpell() { return tempSpell; }

    public void EventTrigger_OnPointerDown() { trigger = true; }
    public void EventTrigger_OnPointerUp() { trigger = false; }
}
