using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Menu_Inventory_MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UI_Menu_Inventory _menu_Inventory;
    [SerializeField] EquipmentType _slotType;

    [Header("UI Element - Selected Window")]
    [SerializeField] GameObject equipItemInfo_Window;
    [SerializeField] Image equipItem_Icon;
    [SerializeField] Text equipItem_Name;
    [SerializeField] Text equipItem_Type;
    [SerializeField] List<Text> equipItem_Specs;

    Item _tempItem;
    
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        switch (_slotType)
        {
            case EquipmentType.Weapon:
                _tempItem = _menu_Inventory.Get_EquipedItem_Weapon();
                break;
            case EquipmentType.Armor:
                _tempItem = _menu_Inventory.Get_EquipedItem_Armor();
                break;
            case EquipmentType.Accessory:
                _tempItem = _menu_Inventory.Get_EquipedItem_Accessory();
                break;
        }
        if (_tempItem == null) return;

        equipItem_Icon.sprite = _tempItem.Get_Sprite_ItemIcon();
        equipItem_Name.text = _tempItem.Get_ItemName();
        equipItem_Type.text = _tempItem.Get_ItemType();
        //Spec
        int specIndex = 0;
        switch (_tempItem.Get_ItemType())
        {
            case "무기":
                if (_tempItem.Get_AdditiveATK() != 0) equipItem_Specs[specIndex++].text = "물리공격력 " + _tempItem.Get_AdditiveATK();
                if (_tempItem.Get_AdditiveMATK() != 0) equipItem_Specs[specIndex++].text = "마법공격력 " + _tempItem.Get_AdditiveMATK();
                break;
            case "방어구":
                if (_tempItem.Get_AdditiveDEF() != 0) equipItem_Specs[specIndex++].text = "물리방어력 " + _tempItem.Get_AdditiveDEF();
                if (_tempItem.Get_AdditiveMDEF() != 0) equipItem_Specs[specIndex++].text = "마법방어력 " + _tempItem.Get_AdditiveMDEF();
                break;
            case "악세사리":
                if (_tempItem.Get_AdditiveSTR() != 0) equipItem_Specs[specIndex++].text = "STR " + _tempItem.Get_AdditiveSTR();
                if (_tempItem.Get_AdditiveCON() != 0) equipItem_Specs[specIndex++].text = "CON " + _tempItem.Get_AdditiveCON();
                if (_tempItem.Get_AdditiveINT() != 0) equipItem_Specs[specIndex++].text = "INT " + _tempItem.Get_AdditiveINT();
                if (_tempItem.Get_AdditiveWIS() != 0) equipItem_Specs[specIndex++].text = "WIS " + _tempItem.Get_AdditiveWIS();
                if (_tempItem.Get_AdditiveDEX() != 0) equipItem_Specs[specIndex++].text = "DEX " + _tempItem.Get_AdditiveDEX();
                break;
        }
        for (int i = specIndex; i < equipItem_Specs.Count; i++) equipItem_Specs[i].text = "";

        equipItemInfo_Window.SetActive(true);
    }
    
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _tempItem = null;
        equipItemInfo_Window.SetActive(false);
    }
}
