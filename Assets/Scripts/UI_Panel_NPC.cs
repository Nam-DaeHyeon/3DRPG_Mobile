using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Panel_NPC : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] UI_Menu_Inventory _menu_Inventory;

    List<Item> _goodList;

    [Header("UI")]
    [SerializeField] List<UI_Panel_NPC_Element> elements;

    public void Load_GoodsList(List<Item> goodList)
    {
        _goodList = goodList;

        for (int i = 0; i < elements.Count; i++)
        {
            if (i < _goodList.Count)
            {
                elements[i].goodsIcon.sprite = _goodList[i].Get_Sprite_ItemIcon();
                elements[i].goodsName.text = _goodList[i].Get_ItemName();
                elements[i].goodsPrice.text = _goodList[i].Get_Price().ToString();

                elements[i].gameObject.SetActive(true);
            }
            else
            {
                elements[i].gameObject.SetActive(false);
            }
        }
    }

    public void UI_Purchase_Item(int itemIndex)
    {
        if (!_player.isPurchased) return;

        StartCoroutine(IE_Purchase_Item(itemIndex));
    }

    private IEnumerator IE_Purchase_Item(int itemIndex)
    {
        //Item item = LoadItem_Database(goodsList[itemIndex].itemId);

        _player.Purchase_Item(_goodList[itemIndex]);
        yield return new WaitUntil(() => _player.isPurchased);

        switch (_menu_Inventory.Get_CurrentSortCategory())
        {
            case SortCategory.Consumables:  //"소모품":
                _menu_Inventory.UI_Sort_Category(2);
                break;
            case SortCategory.Equipment: //"무기": "방어구": "악세사리":
                _menu_Inventory.UI_Sort_Category(1);
                break;
            default:
                _menu_Inventory.UI_Sort_Category(0);
                break;
        }

        _menu_Inventory.Update_PlayerMoney();
        //_menu_Inventory.Update_ItemCount(item);
    }

    public void UI_Close_Panel()
    {
        _goodList = null;
        _menu_Inventory.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
