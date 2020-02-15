using System.Collections.Generic;

public class CharacterData
{
    private string _userId;

    public string nickName;
    public string className;
    public int level;
    public int money;

    public int ab_str;
    public int ab_con;
    public int ab_int;
    public int ab_wis;
    public int ab_dex;

    public int add_ab_str;
    public int add_ab_con;
    public int add_ab_int;
    public int add_ab_wis;
    public int add_ab_dex;

    public int point;

    public List<InventoryItemSet> inventory = new List<InventoryItemSet>();

    public void Add_Item(Item item, int count)
    {
        InventoryItemSet itemset;
               
        switch (item.Get_ItemType().Trim())
        {
            case "소모품": //소모품
                itemset = new InventoryItemSet(item, count);
                inventory.Add(itemset);
                break;
            default: //그 외 : 무기, 방어구, 악세사리
                itemset = new InventoryItemSet(item, 1);
                for (int i = 0; i < count; i++) { inventory.Add(itemset); }
                break;
        }
    }

    public void Add_ItemAmount(InventoryItemSet itemSet, int amount)
    {
        //if (!inventory.Contains(itemSet) && itemSet.item.Get_ItemType().Equals("소모품")) inventory.Add(itemSet);
        //if (!itemSet.item.GetType().Equals("소모품")) inventory.Add(itemSet);

        itemSet.itemCount += amount;

        int index = inventory.IndexOf(itemSet);

        //아이템을 획득할 경우 amount > 0
        //인벤토리에 없을 경우
        if (index == -1) inventory.Add(itemSet);
        //장비(무기, 방어구, 악세사리)일 경우
        else if (itemSet.item.Get_ItemType().Equals("무기") || itemSet.item.Get_ItemType().Equals("무기") || itemSet.item.Get_ItemType().Equals("무기")) inventory.Add(itemSet);

        //아이템을 사용할 경우 amount < 0
        if (itemSet.itemCount <= 0) inventory.RemoveAt(index);

        MySQLManager.instance.Update_ItemCount(itemSet, this);
    }

    public void Add_LEVEL() { level++; }
    public void Add_STR() { ab_str++; }
    public void Add_CON() { ab_con++; }
    public void Add_INT() { ab_int++; }
    public void Add_WIS() { ab_wis++; }
    public void Add_DEX() { ab_dex++; }
    public void Add_POINT() { point++; }
    public void Substract_POINT() { point--; }
    public void Add_Money(int money)
    {
        this.money += money;

        if (money > 99999) money = 99999;
    }

    public void Set_UserId(string userId) { _userId = userId; }
    public string Get_UserId() { return _userId; }

    public int Get_All_STR() { return ab_str + add_ab_str; }
    public int Get_All_CON() { return ab_con + add_ab_con; }
    public int Get_All_INT() { return ab_int + add_ab_int; }
    public int Get_All_WIS() { return ab_wis + add_ab_wis; }
    public int Get_All_DEX() { return ab_dex + add_ab_dex; }
}