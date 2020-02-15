using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu_Status : MonoBehaviour
{
    [SerializeField] Player _player;

    [Header("UI Elements")]
    [SerializeField] Text _levelText;
    [SerializeField] Text _classText;
    [SerializeField] Text _strText;
    [SerializeField] Text _conText;
    [SerializeField] Text _intText;
    [SerializeField] Text _wisText;
    [SerializeField] Text _dexText;
    [SerializeField] Text _pointText;
    [SerializeField] List<Button> _addButtons;

    private void OnEnable()
    {
        Init_Elements();
    }

    private void Init_Elements()
    {
        _levelText.text = GameManager.instance.playerData.level.ToString();
        _classText.text = GameManager.instance.playerData.className;
        _strText.text = GameManager.instance.playerData.ab_str.ToString();
        _conText.text = GameManager.instance.playerData.ab_con.ToString();
        _intText.text = GameManager.instance.playerData.ab_int.ToString();
        _wisText.text = GameManager.instance.playerData.ab_wis.ToString();
        _dexText.text = GameManager.instance.playerData.ab_dex.ToString();
        _pointText.text = GameManager.instance.playerData.point.ToString();

        bool canAdd = false;
        if (GameManager.instance.playerData.point > 0) canAdd = true;
        for (int i = 0; i < _addButtons.Count; i++)
        {
            _addButtons[i].gameObject.SetActive(canAdd);
        }

    }

    public void UI_Button_GivePoint(string abName)
    {
        if (GameManager.instance.playerData.point <= 0) return;
        if (!MySQLManager.instance.Get_EndCheck()) return;

        StartCoroutine(IE_GivePoint(abName));
    }

    private IEnumerator IE_GivePoint(string abName)
    { 
        //연산
        //뷰어 수치 변경
        //서버 연동
        //클라이언트 데이터 동기화

        switch (abName.ToLower())
        {
            case "str":
                GameManager.instance.playerData.Add_STR();
                break;
            case "con":
                GameManager.instance.playerData.Add_CON();
                break;
            case "int":
                GameManager.instance.playerData.Add_INT();
                break;
            case "wis":
                GameManager.instance.playerData.Add_WIS();
                break;
            case "dex":
                GameManager.instance.playerData.Add_DEX();
                break;
        }
        GameManager.instance.playerData.Substract_POINT();

        Init_Elements();

        Debug.Log(GameManager.instance.playerData.level + ": " + GameManager.instance.playerData.ab_int + "/" + GameManager.instance.playerData.ab_wis + " == " + GameManager.instance.playerData.point);
        MySQLManager.instance.Update_PlayingCharacterData();
        yield return new WaitUntil(() => MySQLManager.instance.Get_EndCheck());

        _player.Set_InitDetailStatus();
        _player.Set_FillAmount_HPBar();
        _player.Set_FillAmount_SPBar();
    }
}
