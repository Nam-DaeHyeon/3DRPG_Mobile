using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PrivateItemData
{
    public int _itemId;
    public int _count;

    public void Set_Count(int amount)
    {
        _count += amount;
    }
}

[System.Serializable]
public class ResourceData_CharacterElement
{
    public int level;
    public string className;
    public int ab_str;
    public int ab_con;
    public int ab_int;
    public int ab_wis;
    public int ab_dex;

    public List<PrivateItemData> _inventory;
    public int _money;

    public void Set_Money(int amount)
    {
        _money -= amount;
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }

    public int Get_Money() { return _money; }

    public void Set_ItemCount(Item item, int amount)
    {
        //인벤토리에 있을 경우 : 소모품일 경우
        if (item.Get_ItemType().Equals("소모품"))
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i]._itemId == item.Get_Id())
                {
                    _inventory[i].Set_Count(amount);
                    if (_inventory[i]._count <= 0) _inventory.RemoveAt(i);
#if UNITY_EDITOR
                    AssetDatabase.SaveAssets();
#endif
                    return;
                }
            }
        }
        //인벤토리에 없을 경우
        _inventory.Add(new PrivateItemData() { _itemId = item.Get_Id(), _count = 1 });
    }

    public int Get_ItemCount(Item item)
    {
        //인벤토리에 있을 경우
        for (int i = 0; i < _inventory.Count; i++)
        {
            if (_inventory[i]._itemId == item.Get_Id())
            {
                return _inventory[i]._count;
            }
        }

        return 0;
    }
}

[CreateAssetMenu(fileName = "ResourceData_Character"
    , menuName = "Create Character Data"
    , order = 1)]
public class ResourceData_Character : ScriptableObject
{
    public List<ResourceData_CharacterElement> srcPlayer;
}
