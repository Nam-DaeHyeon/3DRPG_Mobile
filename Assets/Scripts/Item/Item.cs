using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory
}

public class Item
{
    protected int _id;
    protected string _name;
    protected string _itemType;
    protected string _itemComment;

    protected int _healHP = -1;
    protected int _healSP = -1;

    protected int _add_ATK = -1;
    protected int _add_MATK = -1;

    protected int _add_STR = -1;
    protected int _add_CON = -1;
    protected int _add_INT = -1;
    protected int _add_WIS = -1;
    protected int _add_DEX = -1;

    protected int _add_DEF = -1;
    protected int _add_MDEF = -1;

    protected int _price = -1;

    public Item() { }

    public Item(int id, string name, string itemtype, string itemComment,
                    int healHP, int healSP,
                    int addATK, int addMATK,
                    int addSTR, int addCON, int addINT, int addWIS, int addDEX,
                    int addDEF, int addMDEF)
    {
        _id = id;
        _name = name != null ? name : "NoName";
        _itemType = itemtype != null ? itemtype : "ETC";
        _itemComment = itemComment != null ? itemComment : "";

        _healHP = healHP;
        _healSP = healSP;

        _add_ATK = addATK;
        _add_MATK = addMATK;

        _add_STR = addSTR;
        _add_CON = addCON;
        _add_INT = addINT;
        _add_WIS = addWIS;
        _add_DEX = addDEX;

        _add_DEF = addDEF;
        _add_MDEF = addMDEF;
    }

    public void Set_Id(int id) { _id = id; }
    public int Get_Id() { return _id; }
    public void Set_ItemName(string name) { _name = name; }
    public string Get_ItemName() { return _name; }
    public void Set_ItemType(string itemType) { _itemType = itemType; }
    public string Get_ItemType() { return _itemType; }
    public void Set_ItemComment(string itemComment) { _itemComment = itemComment; }
    public string Get_ItemComment() { return _itemComment; }
    public void Set_healHP(int hp) { _healHP = hp; }
    public int Get_healHP() { return _healHP; }
    public void Set_healSP(int sp) { _healSP = sp; }
    public int Get_healSP() { return _healSP; }
    public void Set_AdditiveATK(int atk) { _add_ATK = atk; }
    public int Get_AdditiveATK() { return _add_ATK; }
    public void Set_AdditiveMATK(int matk) { _add_MATK = matk; }
    public int Get_AdditiveMATK() { return _add_MATK; }
    public void Set_AdditiveSTR(int str) { _add_STR = str; }
    public int Get_AdditiveSTR() { return _add_STR; }
    public void Set_AdditiveCON(int con) { _add_CON = con; }
    public int Get_AdditiveCON() { return _add_CON; }
    public void Set_AdditiveINT(int _int) { _add_INT = _int; }
    public int Get_AdditiveINT() { return _add_INT; }
    public void Set_AdditiveWIS(int wis) { _add_WIS = wis; }
    public int Get_AdditiveWIS() { return _add_WIS; }
    public void Set_AdditiveDEX(int dex) { _add_DEX = dex; }
    public int Get_AdditiveDEX() { return _add_DEX; }
    public void Set_AdditiveDEF(int def) { _add_DEF = def; }
    public int Get_AdditiveDEF() { return _add_DEF; }
    public void Set_AdditiveMDEF(int mdef) { _add_MDEF = mdef; }
    public int Get_AdditiveMDEF() { return _add_MDEF; }
    public void Set_Price(int price) { _price = price; }
    public int Get_Price() { return _price; }

    public Sprite Get_Sprite_ItemIcon() { return Resources.Load<Sprite>("Item/" + _name); }
}
