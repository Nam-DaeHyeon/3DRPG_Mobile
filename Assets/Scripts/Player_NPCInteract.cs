using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour
{
    [Header("UI - NPC Interact")]
    [SerializeField] UI_Panel_NPC _npcUI_Window;
    [SerializeField] Button _npcButton;

    public void Set_On_NPC_UI()
    {
        if (!_npcUI_Window.gameObject.activeSelf)
        {
            //targetNPC.LookAt(new Vector3(transform.position.x, targetNPC.position.y, transform.position.z));

            _npcUI_Window.gameObject.SetActive(true);

            btn_MenuON.gameObject.SetActive(false);

            menu_Status.SetActive(false);
            if (!menu_Inventory.activeSelf) menu_Inventory.SetActive(true);
            menu_Spell.SetActive(false);
            menu_Option.SetActive(false);
        }
    }

    public void Set_Off_NPC_UI()
    {
        if (_npcUI_Window.gameObject.activeSelf) _npcUI_Window.gameObject.SetActive(false);
        if (menu_Inventory.activeSelf) menu_Inventory.SetActive(false);
        
        btn_MenuON.gameObject.SetActive(true);
    }

    public void Set_Available_NPC(Transform buttonPos, List<Item> goods)
    {
        _npcUI_Window.Load_GoodsList(goods);

        StartCoroutine(IE_NPCButton_Positioning(buttonPos));
    }

    private IEnumerator IE_NPCButton_Positioning(Transform buttonPos)
    {
        if (_npcButton.gameObject.activeSelf) yield break;

        _npcButton.transform.position = _trackingCam.WorldToScreenPoint(buttonPos.position);
        _npcButton.gameObject.SetActive(true);

        while (_npcButton.gameObject.activeSelf)
        {
            _npcButton.transform.position = _trackingCam.WorldToScreenPoint(buttonPos.position);

            yield return null;
        }
    }

    public void Set_Unavailable_NPC()
    {
        _npcButton.gameObject.SetActive(false);
    }
}
