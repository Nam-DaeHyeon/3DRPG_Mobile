using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour
{
    [Header("Effect")]
    [SerializeField] GameObject _castObj;
    string _inputKey;

    Spell _currSpell;
    InventoryItemSet _currItemSet;

    [Header("Quick Button")]
    [SerializeField] private Transform _mobileInteractGroup;
    [SerializeField] private List<UI_QuickButton> _quicks;
    [SerializeField] private Button btn_quickCancle;

    [HideInInspector] public Spell registSpellTemp;
    [HideInInspector] public InventoryItemSet registItemSetTemp;

    IEnumerator IE_Spell_EndCheck(Spell spell)
    {
        _castObj.transform.position = transform.position;
        _castObj.GetComponent<ObjPosYEditor>().Set_Controller(transform);
        _castObj.SetActive(true);
        yield return new WaitUntil(() => !spell.Get_SpellON());
        isCast = false;
        _castObj.SetActive(false);
    }

    public void Consume_SP(int conSP)
    {
        currSp -= conSP;
        if (currSp < 0) currSp = 0;
    }

    private void Set_InitIconImage()
    {
        Set_Registed_Quicks();

        UI_QuickButton quickBtn;
        Spell tempSpell;
        InventoryItemSet tempItemSet;

        //Disable_InteractableQUICKs();

        for (int i = 0; i < _quicks.Count; i++)
        {
            quickBtn = _quicks[i];

            if (quickBtn.quickLinkedTr == null)
            {
                _quicks[i].quickIconImage.enabled = false;
                quickBtn.quickTimer.fillAmount = 0;
                _quicks[i].quickItemCount.text = "";

                continue;
            }
            if (quickBtn.quickLinkedTr.GetComponent<Spell>())
            {
                tempSpell = quickBtn.quickLinkedTr.GetComponent<Spell>();

                quickBtn.quickIconImage.sprite = tempSpell.Get_Sprite_SpellIcon();
                quickBtn.quickIconImage.enabled = true;

                if (tempSpell.Get_CurrentCoolTime() <= 0) quickBtn.quickTimer.fillAmount = 0;
                else quickBtn.quickTimer.fillAmount = tempSpell.Get_CurrentCoolTime() / tempSpell.Get_MaxCoolTime();
                quickBtn.Start_Timer(tempSpell);
            }
            else if(_quicks[i].Get_RegisteredItemSet() != null)
            {
                tempItemSet = _quicks[i].Get_RegisteredItemSet();

                quickBtn.quickIconImage.sprite = tempItemSet.item.Get_Sprite_ItemIcon();
                quickBtn.quickIconImage.enabled = true;

                //아이템 쿨타임 초기화
                //if (tempItem.Get_CurrentCoolTime() <= 0) quickBtn.quickTimer.text = "";
                //else quickBtn.quickTimer.text = ((int)tempItem.Get_CurrentCoolTime()).ToString();
                quickBtn.Start_Timer(tempItemSet);
            }
            else
            {
                _quicks[i].quickIconImage.enabled = false;
                _quicks[i].quickTimer.fillAmount = 0;
            }
        }
    }
    
    public void UI_Button_Press_QuickButton(int quickIndex)
    {
        //아무것도 등록된 것이 없습니다. || 덮어쓰기.
        if (_quicks[quickIndex].Get_RegisteredItemSet() == null && _quicks[quickIndex].Get_RegisteredSpell() == null)
        {
            _quicks[quickIndex].Set_QuickElement();
            return;
        }

        if(_quicks[quickIndex].Get_RegisteredSpell() != null)
        {
            if (isJump) return;
            if (isCast) return;
            
            _currSpell = _quicks[quickIndex].Get_RegisteredSpell();

            if (_currSpell.Get_CurrentCoolTime() > 0) return;
            if (_currSpell.Get_ConsumeSP() > currSp) return;
            currSp -= _currSpell.Get_ConsumeSP();
            Set_FillAmount_SPBar();
            
            isCast = true;
            _currSpell.Set_SpellCaster(this);
            _currSpell.Cast(_quicks[quickIndex], _castObj.transform);
            StartCoroutine(IE_Spell_EndCheck(_currSpell));
        }

        if (_quicks[quickIndex].Get_RegisteredItemSet() != null)
        {
            if (isCast) return;

            _currItemSet = _quicks[quickIndex].Get_RegisteredItemSet();

            //아이템 쿨타임 중일 때 기능 중지
            if (_currItemSet.Get_CurrentCoolTime() > 0) return;
            if (_currItemSet.itemCount < 1) return;

            Use_Potion(_currItemSet.item.Get_healHP(), _currItemSet.item.Get_healSP());

            GameManager.instance.playerData.Add_ItemAmount(_currItemSet, -1);
            Update_QuickSlot_ItemCount(_quicks[quickIndex]);

            if(_currItemSet.itemCount <= 0)
            {
                _currItemSet = null;

                _quicks[quickIndex].quickIconImage.enabled = false;
                _quicks[quickIndex].quickTimer.fillAmount = 0;
            }
        }
    }

    //퀵버튼 요소를 등록하는 중입니다.
    public void Set_Registing_Quicks()
    {
        //취소버튼 활성화
        btn_quickCancle.gameObject.SetActive(true);

        //상호작용 버튼들이 가장 위로 배치되도록 로컬 배치 순서 변경
        _mobileInteractGroup.transform.SetAsLastSibling();
    }

    //퀵버튼 요소를 등록하는 과정을 취소합니다.
    public void UI_Cancle_RegistingQuickButton()
    {
        Set_Registed_Quicks();
    }

    public void Set_Registed_Quicks()
    {
        registSpellTemp = null;
        registItemSetTemp = null;

        btn_quickCancle.gameObject.SetActive(false);
        
        //상호작용 버튼들이 가장 뒤로 배치되도록 로컬 배치 순서 변경
        _mobileInteractGroup.transform.SetAsFirstSibling();
    }

    public void Update_QuickSlot_ItemCount(UI_QuickButton tempQuicks)
    {
        UI_Menu_Inventory menuInven = menu_Inventory.GetComponent<UI_Menu_Inventory>();

        if (tempQuicks.Get_RegisteredItemSet() != null)
        {
            //tempQuicks.quickItemCount.text = GameManager.instance.playerData.Get_ItemCount(tempQuicks.Get_RegisteredItem()).ToString();
            tempQuicks.quickItemCount.text = tempQuicks.Get_RegisteredItemSet().itemCount.ToString();

            //menuInven.Update_ItemCount(tempQuicks.Get_RegisteredItem());
        }
    }

    public void Update_QuickSlot_ItemCount()
    {
        for (int i = 0; i < _quicks.Count; i++)
        {
            if (_quicks[i].Get_RegisteredItemSet() != null)
            {
                //tempQuicks[i].quickItemCount.text = GameManager.instance.playerData.Get_ItemCount(tempQuicks[i].Get_RegisteredItem()).ToString();
                _quicks[i].quickItemCount.text = _quicks[i].Get_RegisteredItemSet().itemCount.ToString();
            }
        }
    }
}
