using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour
{
    [Header("Menu UI Manager")]
    [SerializeField] GameObject btn_MenuON;
    [SerializeField] GameObject group_Menu;
    [SerializeField] GameObject menu_Status;
    [SerializeField] GameObject menu_Inventory;
    [SerializeField] GameObject menu_Spell;
    [SerializeField] GameObject menu_Option;

    [SerializeField] GameObject deadScene;
    [SerializeField] Text deadScene_Count;

    public void UI_Click_Status()
    {
        if (!menu_Status.activeSelf)
        {
            menu_Status.SetActive(true);
        }
        else
        {
            menu_Status.SetActive(false);
        }

        menu_Inventory.SetActive(false);
        menu_Spell.SetActive(false);
        menu_Option.SetActive(false);
    }

    public void UI_Click_Inventory()
    {
        if (!menu_Inventory.activeSelf)
        {
            menu_Inventory.SetActive(true);
        }
        else
        {
            menu_Inventory.SetActive(false);
            _npcUI_Window.gameObject.SetActive(false);
        }

        menu_Status.SetActive(false);
        menu_Spell.SetActive(false);
        menu_Option.SetActive(false);
    }

    public void UI_Click_Spell()
    {
        if (!menu_Spell.activeSelf)
        {
            menu_Spell.SetActive(true);
        }
        else
        {
            menu_Spell.SetActive(false);
        }

        menu_Status.SetActive(false);
        menu_Inventory.SetActive(false);
        menu_Option.SetActive(false);
    }

    public void UI_Click_Option()
    {
        if (!menu_Option.activeSelf)
        {
            menu_Option.SetActive(true);
        }
        else
        {
            menu_Option.SetActive(false);
        }

        menu_Status.SetActive(false);
        menu_Inventory.SetActive(false);
        menu_Spell.SetActive(false);
    }

    public void UI_Click_MenuONOFF()
    {
        //Order MENU ON
        if(btn_MenuON.activeSelf)
        {
            btn_MenuON.SetActive(false);
            group_Menu.SetActive(true);
        }
        else    //Order MENU OFF
        {
            btn_MenuON.SetActive(true);
            group_Menu.SetActive(false);

            menu_Status.SetActive(false);
            menu_Inventory.SetActive(false);
            menu_Spell.SetActive(false);
            menu_Option.SetActive(false);
        }
    }

    public void UI_Click_NPCButton()
    {
        if(!_npcUI_Window.gameObject.activeSelf)
        {
            Set_On_NPC_UI();
        }
        else
        {
            Set_Off_NPC_UI();
        }
    }

    private void Event_Dead()
    {
        StartCoroutine(IE_Event_Dead());
    }

    private IEnumerator IE_Event_Dead()
    {
        deadScene.SetActive(true);

        for (int i = 5; i > 0; i--)
        {
            deadScene_Count.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        deadScene.SetActive(false);

        Event_Alive();
    }
}
