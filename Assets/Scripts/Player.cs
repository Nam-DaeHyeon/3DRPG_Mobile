using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public enum PlayerState
    {
        NONE,
        DEAD
    }

    public PlayerState p_State;

    [SerializeField] float currHp;
    [SerializeField] float maxHp;
    [SerializeField] float currSp;
    [SerializeField] float maxSp;
    
    [SerializeField] float _attack;
    [SerializeField] float _Mattack;
    [SerializeField] float _criticalRate;
    [SerializeField] float _attackAngle = 60;

    [SerializeField] float _defence;
    [SerializeField] float _Mdefence;
    bool buff_DivideDamage;

    public Item equip_Weapon;
    public Item equip_Armor;
    public Item equip_Accessory;

    [Header("Attacking Process")]
    [SerializeField] PlayerMeleeCollider _meleeCol;
    [SerializeField] float _meleeRange;
    [SerializeField] PlayerSpellCollider _spellCol;
    Monster _targetMonster;

    public bool isPurchased = true;

    private void Start()
    {
        Receive_LobbyData();

        Set_InitIconImage();

        Set_InitDetailStatus();

        Event_Alive();
    }

    private void Event_Alive()
    {
        p_State = PlayerState.NONE;

        if (StageManager.instance != null)
        {
            transform.position = StageManager.instance.playerSpawnPos.position;
            transform.rotation = StageManager.instance.playerSpawnPos.rotation;
        }

        currHp = maxHp;
        currSp = maxSp;
        Set_FillAmount_HPBar();
        Set_FillAmount_SPBar();

        _meleeCol.gameObject.SetActive(false);
        _meleeCol.Set_Radius(_meleeRange);

        //카메라 위치 초기화
        if(!_initCamPos.Equals(Vector3.zero)) _trackingCam.gameObject.transform.position = _initCamPos;

        //UI 초기화
        _npcButton.gameObject.SetActive(false);

        StartCoroutine(IE_Track_Camera());
        StartCoroutine(IE_Controller());
    }

    private void Receive_LobbyData()
    {
        //파라미터 정의.
        string _playerClass = GameManager.instance.playerData.className;
        switch(_playerClass.ToLower())
        {
            case "warrior":
                gameObject.AddComponent<gmClass_Warrior>();
                break;
            case "magician":
                gameObject.AddComponent<gmClass_Magician>();
                break;
        }

        //모델&커스텀 정의.
        //GameObject model = Instantiate(GameManager.instance.selectedCharacterModel);
        GameObject model = GameManager.instance.selectedCharacterModel;
        model.transform.position = transform.position;
        model.transform.rotation = transform.rotation;
        model.transform.parent = transform;

        _animator = model.GetComponent<Animator>();
    }

    public void Set_InitDetailStatus()
    {
        maxHp = 100 + GameManager.instance.playerData.Get_All_CON() * 30;
        maxSp = 30 + GameManager.instance.playerData.Get_All_WIS() * 30;

        _attack = 10 + GameManager.instance.playerData.Get_All_STR() * 2;
        _Mattack = 10 + GameManager.instance.playerData.Get_All_INT() * 5;

        _criticalRate = 5 + GameManager.instance.playerData.Get_All_DEX();
    }

    public void TakeDamage(float damage)
    {
        if (p_State.Equals(PlayerState.DEAD)) return;

        float EndDamage = damage - _defence;
        if (EndDamage <= 1) EndDamage = 1;

        if (buff_DivideDamage) EndDamage *= 0.5f;

        currHp -= EndDamage;

        if(currHp <= 0)
        {
            p_State = PlayerState.DEAD;
            currHp = 0;

            Event_Dead();
        }
        else
        {

        }

        HUDMessageManager.instance.Load_TextMessage(transform.position, EndDamage.ToString());
        Set_FillAmount_HPBar();
    }

    public void TakeHeal(float healAmount)
    {
        if (p_State.Equals(PlayerState.DEAD)) return;

        currHp += healAmount;

        if (currHp > maxHp) currHp = maxHp;

        HUDMessageManager.instance.Load_TextMessage(transform.position, healAmount.ToString());
        Set_FillAmount_HPBar();
    }

    public void Use_Potion(int HHP, int HSP)
    {
        currHp += (maxHp * 0.01f * HHP);
        if (currHp >= maxHp) currHp = maxHp;
        currSp += (maxSp * 0.01f * HSP);
        if (currSp >= maxSp) currSp = maxSp;
        Set_FillAmount_HPBar();
        Set_FillAmount_SPBar();
    }

    public void Equip_Item(Item item, EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                equip_Weapon = item;
                break;
            case EquipmentType.Armor:
                equip_Armor = item;
                break;
            case EquipmentType.Accessory:
                equip_Accessory = item;
                break;
        }

        _attack += item.Get_AdditiveATK();
        _Mattack += item.Get_AdditiveMATK();

        GameManager.instance.playerData.add_ab_str += item.Get_AdditiveSTR();
        GameManager.instance.playerData.add_ab_con += item.Get_AdditiveCON();
        GameManager.instance.playerData.add_ab_int += item.Get_AdditiveINT();
        GameManager.instance.playerData.add_ab_wis += item.Get_AdditiveWIS();
        GameManager.instance.playerData.add_ab_dex += item.Get_AdditiveDEX();

        _defence += item.Get_AdditiveDEF();
        _Mdefence += item.Get_AdditiveMDEF();
    }

    public void Dequip_Item(EquipmentType type)
    {
        Item item = null;
        switch (type)
        {
            case EquipmentType.Weapon:
                item = equip_Weapon;
                break;
            case EquipmentType.Armor:
                item = equip_Armor;
                break;
            case EquipmentType.Accessory:
                item = equip_Accessory;
                break;
        }
        if (item == null) return;

        _attack -= item.Get_AdditiveATK();
        _Mattack -= item.Get_AdditiveMATK();

        GameManager.instance.playerData.add_ab_str -= item.Get_AdditiveSTR();
        GameManager.instance.playerData.add_ab_con -= item.Get_AdditiveCON();
        GameManager.instance.playerData.add_ab_int -= item.Get_AdditiveINT();
        GameManager.instance.playerData.add_ab_wis -= item.Get_AdditiveWIS();
        GameManager.instance.playerData.add_ab_dex -= item.Get_AdditiveDEX();
        
        _defence -= item.Get_AdditiveDEF();
        _Mdefence -= item.Get_AdditiveMDEF();
    }

    public void Purchase_Item(Item item)
    {
        if (!isPurchased) return;
        isPurchased = false;

        //인벤토리 공간 확인.
        if (GameManager.instance.playerData.inventory.Count >= menu_Inventory.GetComponent<UI_Menu_Inventory>().Get_Count_ItemSlots())
        {
            return;
        }

        //재화 조건 확인.
        if (GameManager.instance.playerData.money < item.Get_Price())
        {
            return;
        }

        StartCoroutine(IE_Purchase_Item(item));
    }

    private IEnumerator IE_Purchase_Item(Item item)
    {
        //아이템 가격 정산.
        //GameManager.instance.playerData.money -= item.Get_Price();
        GameManager.instance.playerData.Add_Money(-item.Get_Price());
        MySQLManager.instance.Update_PlayingCharacterData();
        yield return new WaitUntil(() => MySQLManager.instance.Get_EndCheck());

        //아이템 인벤토리 추가.
        //인벤토리 내에 인덱스를 알아낸다.
        InventoryItemSet itemset = null;
        if (item.Get_ItemType().Equals("소모품"))
        {
            for (int i = 0; i < GameManager.instance.playerData.inventory.Count; i++)
            {
                if (GameManager.instance.playerData.inventory[i].item.Get_Id() == item.Get_Id())
                {
                    itemset = GameManager.instance.playerData.inventory[i];
                    break;
                }
            }
        }

        if (itemset == null) itemset = new InventoryItemSet(item, 0);

        GameManager.instance.playerData.Add_ItemAmount(itemset, 1);
        yield return new WaitUntil(() => MySQLManager.instance.Get_EndCheck());

        isPurchased = true;
    }
    
    public float Get_CurrentHP() { return currHp; }
    public float Get_MaxHP() { return maxHp; }
    public void Set_Buff_DivideDamage(bool setup) { buff_DivideDamage = setup; }

    public List<Spell> Get_ClassSpell()
    {
        gmClass myClass = GetComponent<gmClass>();

        return myClass.Get_ClassSpell();
    }

    public void Active_SpellCollider(float radius, Vector3 pos)
    {
        _spellCol.Set_Sphere_Radius(radius);
        _spellCol.transform.position = pos;
        _spellCol.gameObject.SetActive(true);
    }

    public void Unactive_SpellCollider()
    {
        _spellCol.gameObject.SetActive(false);
    }

    public void Calculate_Spell_HitMonster(Spell spell)
    {
        for(int i = 0; i < _spellCol.Get_Targets().Count; i++)
        {
            _spellCol.Get_Targets()[i].TakeDamage(_Mattack * spell.Get_BonusMulti() + spell.Get_BonusAdd());
            if (_spellCol.Get_Targets()[i].Get_CurrentHP() <= 0) Get_Exp_Money();
        }
    }

    public void Calculate_Spell_HitPlayer(Spell spell)
    {
        for (int i = 0; i < _spellCol.Get_Players().Count; i++)
        {
            _spellCol.Get_Players()[i].TakeHeal(_Mattack * spell.Get_BonusMulti() + spell.Get_BonusAdd());
        }
    }
}
