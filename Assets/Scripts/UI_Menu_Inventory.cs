using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


[SerializeField]
public enum SortCategory
{
    All,
    Consumables,
    Equipment
}

public class UI_Menu_Inventory : MonoBehaviour
{
    [SerializeField] Player _player;

    [System.Serializable]
    struct EquipSlot
    {
        public Image equipIcon;
        public Item equipItem;
    }

    [System.Serializable]
    class ItemSlot
    {
        public Button itemButton;
        public Image itemImage;
        public Text itemCountText;

        public InventoryItemSet itemSet;
    }

    [Header("UI Element")]
    [SerializeField] EquipSlot equipSlot_Weapon;
    [SerializeField] EquipSlot equipSlot_Armor;
    [SerializeField] EquipSlot equipSlot_Accessory;
    [SerializeField] List<ItemSlot> _itemSlots;
    [SerializeField] Text _moneyText;

    [Header("UI Element - Selected Window")]
    [SerializeField] GameObject selectedItem_WindowObj;
    [SerializeField] Image selectedItem_Icon;
    [SerializeField] Text selectedItem_Name;
    [SerializeField] Text selectedItem_Type;
    [SerializeField] List<Text> selectedItem_Specs;
    [SerializeField] Text selectedItem_Comment;
    [SerializeField] Button selectedItem_ToolQuick;
    [SerializeField] Button selectedItem_ToolEquip;
    [SerializeField] Button selectedItem_ToolDequip;
    [SerializeField] Button selectedItem_ToolUse;
    
    InventoryItemSet _selectItemSet;
    SortCategory _currCategory;

    bool isUsed = true;

    private void OnEnable()
    {
        Init_Elements();
    }

    public void Init_Elements()
    {
        //List<Dictionary<string, object>> ItemList = CSVReader.Read("ItemList");
        Item tempItem;
        //_invenItemList.Clear();

        selectedItem_WindowObj.SetActive(false);

        //Equipment Element
        #region Equipment Setting
        if (_player.equip_Weapon == null) equipSlot_Weapon.equipIcon.gameObject.SetActive(false);
        else
        {
            tempItem = _player.equip_Weapon;

            equipSlot_Weapon.equipIcon.sprite = tempItem.Get_Sprite_ItemIcon();
            equipSlot_Weapon.equipIcon.gameObject.SetActive(true);
        }
        if (_player.equip_Armor == null) equipSlot_Armor.equipIcon.gameObject.SetActive(false);
        else
        {
            tempItem = _player.equip_Armor;

            equipSlot_Armor.equipIcon.sprite = tempItem.Get_Sprite_ItemIcon();
            equipSlot_Armor.equipIcon.gameObject.SetActive(true);
        }
        if (_player.equip_Accessory == null) equipSlot_Accessory.equipIcon.gameObject.SetActive(false);
        else
        {
            tempItem = _player.equip_Accessory;

            equipSlot_Accessory.equipIcon.sprite = tempItem.Get_Sprite_ItemIcon();
            equipSlot_Accessory.equipIcon.gameObject.SetActive(true);
        }
        #endregion

        //Sort_ALL_Id_DESC();
        UI_Sort_Category();
    }

    /// <summary>
    /// At Unity UGUI, this method operates as a Button Function.
    /// </summary>
    /// <param name="sortCode">0 : ALL, 1 : 무기&방어구&악세사리, 2 : 소모품</param>
    public void UI_Sort_Category(int sortCode = 0)
    {
        switch (sortCode)
        {
            case 1:
                _currCategory = SortCategory.Equipment;
                Sort_Equipment();
                break;
            case 2:
                _currCategory = SortCategory.Consumables;
                Sort_Consumables();
                break;
            default:
                _currCategory = SortCategory.All;
                Sort_ALL_Id_DESC();
                break;
        }
    }

    private void Sort_Equipment()
    {
        InventoryItemSet tempItemSet;

        var tempQry = from node in GameManager.instance.playerData.inventory
                      where node.item.Get_ItemType().Equals("무기") ||
                            node.item.Get_ItemType().Equals("방어구") ||
                            node.item.Get_ItemType().Equals("악세사리")
                      orderby node.item.Get_Id() ascending
                      select node;

        //GameManager.instance.playerData.inventory = tempQry.ToList();

        #region 슬롯 초기화
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            tempItemSet = null;

            if (i >= tempQry.ToList().Count)
            {
                _itemSlots[i].itemImage.gameObject.SetActive(false);
                _itemSlots[i].itemCountText.text = "";
            }
            else
            {
                tempItemSet = tempQry.ToList()[i];

                _itemSlots[i].itemSet = tempItemSet;

                _itemSlots[i].itemImage.sprite = _itemSlots[i].itemSet.item.Get_Sprite_ItemIcon();
                _itemSlots[i].itemImage.gameObject.SetActive(true);

                _itemSlots[i].itemCountText.gameObject.SetActive(false);
            }
        }
        #endregion

        Update_PlayerMoney();
    }

    private void Sort_Consumables()
    {
        InventoryItemSet tempItemSet;

        var tempQry = from node in GameManager.instance.playerData.inventory
                      where node.item.Get_ItemType().Equals("소모품")
                      select node;

        //GameManager.instance.playerData.inventory = tempQry.ToList();

        for (int i = 0; i < _itemSlots.Count; i++)
        {
            tempItemSet = null;

            if (i >= tempQry.ToList().Count)
            {
                _itemSlots[i].itemImage.gameObject.SetActive(false);
                _itemSlots[i].itemCountText.text = "";
            }
            else
            {
                tempItemSet = tempQry.ToList()[i];

                _itemSlots[i].itemSet = tempItemSet;

                _itemSlots[i].itemImage.sprite = _itemSlots[i].itemSet.item.Get_Sprite_ItemIcon();
                _itemSlots[i].itemImage.gameObject.SetActive(true);

                _itemSlots[i].itemCountText.text = _itemSlots[i].itemSet.itemCount.ToString();
                _itemSlots[i].itemCountText.gameObject.SetActive(true);
            }
        }

        Update_PlayerMoney();
    }

    private void Sort_ALL_Id_DESC()
    {
        InventoryItemSet tempItemSet;

        var tempQry = from node in GameManager.instance.playerData.inventory
                      orderby node.item.Get_Id() descending
                      select node;

        //GameManager.instance.playerData.inventory = tempQry.ToList();

        //Inventory Element
        //List<InventoryItemSet> playerInventory = GameManager.instance.playerData.inventory;
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            tempItemSet = null;

            //if (i >= playerInventory.Count)
            if (i >= tempQry.ToList().Count)
            {
                _itemSlots[i].itemImage.gameObject.SetActive(false);
                _itemSlots[i].itemCountText.text = "";
            }
            else
            {
                //tempItemSet = playerInventory[i];
                tempItemSet = tempQry.ToList()[i];

                _itemSlots[i].itemSet = tempItemSet;

                _itemSlots[i].itemImage.sprite = _itemSlots[i].itemSet.item.Get_Sprite_ItemIcon();
                _itemSlots[i].itemImage.gameObject.SetActive(true);

                _itemSlots[i].itemCountText.text = _itemSlots[i].itemSet.itemCount.ToString();
                if (_itemSlots[i].itemSet.item.Get_ItemType().Equals("소모품")) _itemSlots[i].itemCountText.gameObject.SetActive(true);
                else _itemSlots[i].itemCountText.gameObject.SetActive(false);
            }
        }

        Update_PlayerMoney();
    }

    public void UI_Show_ItemInformation(int invenIndex)
    {
        //if (invenIndex >= _invenItemList.Count) return;
        _selectItemSet = _itemSlots[invenIndex].itemSet;

        selectedItem_Icon.sprite = _selectItemSet.item.Get_Sprite_ItemIcon();
        selectedItem_Name.text = _selectItemSet.item.Get_ItemName();
        if (_selectItemSet.itemCount != 1) selectedItem_Name.text += " (" + _selectItemSet.itemCount + ")";

        selectedItem_Type.text = _selectItemSet.item.Get_ItemType();

        //Spec
        int specIndex = 0;
        selectedItem_ToolEquip.gameObject.SetActive(false);
        selectedItem_ToolDequip.gameObject.SetActive(false);
        selectedItem_ToolQuick.gameObject.SetActive(false);
        selectedItem_ToolUse.gameObject.SetActive(false);
        switch (_selectItemSet.item.Get_ItemType())
        {
            case "소모품":
                selectedItem_ToolQuick.gameObject.SetActive(true);
                selectedItem_ToolUse.gameObject.SetActive(true);
                if (_selectItemSet.item.Get_healHP() != 0) selectedItem_Specs[specIndex++].text = "HP 회복 " + _selectItemSet.item.Get_healHP();
                if (_selectItemSet.item.Get_healSP() != 0) selectedItem_Specs[specIndex++].text = "SP 회복 " + _selectItemSet.item.Get_healSP();
                break;
            case "무기":
                if (equipSlot_Weapon.equipItem != _selectItemSet.item) selectedItem_ToolEquip.gameObject.SetActive(true);
                else selectedItem_ToolDequip.gameObject.SetActive(true);
                if (_selectItemSet.item.Get_AdditiveATK() != 0) selectedItem_Specs[specIndex++].text = "물리공격력 " + _selectItemSet.item.Get_AdditiveATK();
                if (_selectItemSet.item.Get_AdditiveMATK() != 0) selectedItem_Specs[specIndex++].text = "마법공격력 " + _selectItemSet.item.Get_AdditiveMATK();
                break;
            case "방어구":
                if (equipSlot_Armor.equipItem != _selectItemSet.item) selectedItem_ToolEquip.gameObject.SetActive(true);
                else selectedItem_ToolDequip.gameObject.SetActive(true);
                if (_selectItemSet.item.Get_AdditiveDEF() != 0) selectedItem_Specs[specIndex++].text = "물리방어력 " + _selectItemSet.item.Get_AdditiveDEF();
                if (_selectItemSet.item.Get_AdditiveMDEF() != 0) selectedItem_Specs[specIndex++].text = "마법방어력 " + _selectItemSet.item.Get_AdditiveMDEF();
                break;
            case "악세사리":
                if (equipSlot_Accessory.equipItem != _selectItemSet.item) selectedItem_ToolEquip.gameObject.SetActive(true);
                else selectedItem_ToolDequip.gameObject.SetActive(true);
                if (_selectItemSet.item.Get_AdditiveSTR() != 0) selectedItem_Specs[specIndex++].text = "STR " + _selectItemSet.item.Get_AdditiveSTR();
                if (_selectItemSet.item.Get_AdditiveCON() != 0) selectedItem_Specs[specIndex++].text = "CON " + _selectItemSet.item.Get_AdditiveCON();
                if (_selectItemSet.item.Get_AdditiveINT() != 0) selectedItem_Specs[specIndex++].text = "INT " + _selectItemSet.item.Get_AdditiveINT();
                if (_selectItemSet.item.Get_AdditiveWIS() != 0) selectedItem_Specs[specIndex++].text = "WIS " + _selectItemSet.item.Get_AdditiveWIS();
                if (_selectItemSet.item.Get_AdditiveDEX() != 0) selectedItem_Specs[specIndex++].text = "DEX " + _selectItemSet.item.Get_AdditiveDEX();
                break;
        }
        for (int i = specIndex; i < selectedItem_Specs.Count; i++) selectedItem_Specs[i].text = "";

        selectedItem_Comment.text = _selectItemSet.item.Get_ItemComment();

        selectedItem_WindowObj.SetActive(true);
    }

    public void UI_Hide_ItemInformation()
    {
        _selectItemSet = null;

        selectedItem_WindowObj.SetActive(false);
    }

    public void UI_QuickRegist_Item()
    {
        if (_player.registSpellTemp == null)
        {
            _player.registItemSetTemp = _selectItemSet;

            //_player.Enable_InteractableQUICKs();
            _player.Set_Registing_Quicks();
        }
        else
        {
            _player.registItemSetTemp = null;

            //_player.Disable_InteractableQUICKs();
            _player.Set_Registed_Quicks();
        }

    }

    public void UI_Use_Item()
    {
        if (!isUsed) return;
        isUsed = false;

        StartCoroutine(IE_Use_Item());
    }

    private IEnumerator IE_Use_Item()
    {
        Debug.Log(_selectItemSet.item.Get_ItemName() + ": " + _selectItemSet.item.Get_healHP() + ", " + _selectItemSet.item.Get_healSP());
        _player.Use_Potion(_selectItemSet.item.Get_healHP(), _selectItemSet.item.Get_healSP());
        GameManager.instance.playerData.Add_ItemAmount(_selectItemSet, -1);
        yield return new WaitUntil(() => MySQLManager.instance.Get_EndCheck());
        
        if (_selectItemSet.itemCount <= 0)
        {
            UI_Hide_ItemInformation();
            _selectItemSet = null;
        }
        else
        {
            selectedItem_Name.text = _selectItemSet.item.Get_ItemName();
            selectedItem_Name.text += " (" + _selectItemSet.itemCount + ")";
        }

        switch (_currCategory)
        {
            case SortCategory.All:
                Sort_ALL_Id_DESC();
                break;
            case SortCategory.Consumables:
                Sort_Consumables();
                break;
        }

        isUsed = true;
    }

    public void UI_Equip_Item()
    {
        switch (_selectItemSet.item.Get_ItemType())
        {
            case "무기":
                if (equipSlot_Weapon.equipItem != _selectItemSet.item)
                {
                    _player.Dequip_Item(EquipmentType.Weapon);
                    equipSlot_Weapon.equipIcon.sprite = null;
                    equipSlot_Weapon.equipItem = null;

                    equipSlot_Weapon.equipIcon.sprite = _selectItemSet.item.Get_Sprite_ItemIcon();
                    equipSlot_Weapon.equipIcon.gameObject.SetActive(true);
                    equipSlot_Weapon.equipItem = _selectItemSet.item;

                    _player.Equip_Item(_selectItemSet.item, EquipmentType.Weapon);
                }
                break;
            case "방어구":
                if (equipSlot_Armor.equipItem != _selectItemSet.item)
                {
                    _player.Dequip_Item(EquipmentType.Armor);
                    equipSlot_Armor.equipIcon.sprite = null;
                    equipSlot_Armor.equipItem = null;

                    equipSlot_Armor.equipIcon.sprite = _selectItemSet.item.Get_Sprite_ItemIcon();
                    equipSlot_Armor.equipIcon.gameObject.SetActive(true);
                    equipSlot_Armor.equipItem = _selectItemSet.item;
                    _player.Equip_Item(_selectItemSet.item, EquipmentType.Armor);
                }
                break;
            case "악세사리":
                if (equipSlot_Accessory.equipItem != _selectItemSet.item)
                {
                    _player.Dequip_Item(EquipmentType.Accessory);
                    equipSlot_Accessory.equipIcon.sprite = null;
                    equipSlot_Accessory.equipItem = null;

                    equipSlot_Accessory.equipIcon.sprite = _selectItemSet.item.Get_Sprite_ItemIcon();
                    equipSlot_Accessory.equipIcon.gameObject.SetActive(true);
                    equipSlot_Accessory.equipItem = _selectItemSet.item;
                    _player.Equip_Item(_selectItemSet.item, EquipmentType.Accessory);
                }
                break;
        }

        _player.Set_InitDetailStatus();

        selectedItem_ToolEquip.gameObject.SetActive(false);
        selectedItem_ToolDequip.gameObject.SetActive(true);
    }

    public void UI_Dequip_Item()
    {
        switch (_selectItemSet.item.Get_ItemType())
        {
            case "무기":
                if (equipSlot_Weapon.equipItem == _selectItemSet.item)
                {
                    _player.Dequip_Item(EquipmentType.Weapon);
                    equipSlot_Weapon.equipIcon.sprite = null;
                    equipSlot_Weapon.equipItem = null;

                    equipSlot_Weapon.equipIcon.gameObject.SetActive(false);
                }
                break;
            case "방어구":
                if (equipSlot_Armor.equipItem == _selectItemSet.item)
                {
                    _player.Dequip_Item(EquipmentType.Armor);
                    equipSlot_Armor.equipIcon.sprite = null;
                    equipSlot_Armor.equipItem = null;

                    equipSlot_Armor.equipIcon.gameObject.SetActive(false);
                }
                break;
            case "악세사리":
                if (equipSlot_Accessory.equipItem == _selectItemSet.item)
                {
                    _player.Dequip_Item(EquipmentType.Accessory);
                    equipSlot_Accessory.equipIcon.sprite = null;
                    equipSlot_Accessory.equipItem = null;

                    equipSlot_Accessory.equipIcon.gameObject.SetActive(false);
                }
                break;
        }

        _player.Set_InitDetailStatus();

        selectedItem_ToolEquip.gameObject.SetActive(true);
        selectedItem_ToolDequip.gameObject.SetActive(false);
    }

    public void Update_PlayerMoney()
    {
        _moneyText.text = GameManager.instance.playerData.money.ToString();
    }

    public void Update_ItemCount(Item item)
    {
        //퀵버튼 아이템 갯수 갱신
        _player.Update_QuickSlot_ItemCount();

        //선택창 아이템 갯수 갱신
        if (item.Equals(_selectItemSet.item))
        {
            if (selectedItem_WindowObj.activeSelf)
            {
                //if (GameManager.instance.playerData.Get_ItemCount(item) <= 0) selectedItem_WindowObj.SetActive(false);
                if (_selectItemSet.itemCount <= 0) selectedItem_WindowObj.SetActive(false);
                selectedItem_Name.text = item.Get_ItemName();
                //selectedItem_Name.text += " (" + GameManager.instance.playerData.Get_ItemCount(item) + ")";
                selectedItem_Name.text += " (" + _selectItemSet.itemCount + ")";
            }
        }

        //인벤토리 슬롯 아이템 갯수 갱신
        //for(int i = 0; i < _invenItemList.Count; i++)
        for(int i = 0; i < _itemSlots.Count; i++)
        {
            //if(item.Get_Id() == _invenItemList[i].Get_Id())
            if(item.Get_Id() == _itemSlots[i].itemSet.item.Get_Id())
            {
                if(gameObject.activeSelf)
                {
                    switch (_currCategory)
                    {
                        case SortCategory.All:
                            Sort_ALL_Id_DESC();
                            break;
                        case SortCategory.Consumables:
                            Sort_Consumables();
                            break;
                        case SortCategory.Equipment:
                            Sort_Equipment();
                            break;
                    }
                }
            }
        }
    }

    public Item Get_EquipedItem_Weapon() { return equipSlot_Weapon.equipItem; }
    public Item Get_EquipedItem_Armor() { return equipSlot_Armor.equipItem; }
    public Item Get_EquipedItem_Accessory() { return equipSlot_Accessory.equipItem; }
    public SortCategory Get_CurrentSortCategory() { return _currCategory; }
    public int Get_Count_ItemSlots() { return _itemSlots.Count; }

}