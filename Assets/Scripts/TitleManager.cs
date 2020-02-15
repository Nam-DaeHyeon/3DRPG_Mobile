using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GooglePlayManager _gpManager;

    private void Start()
    {
        SoundManager.instance.Play_BG_Title();
    }

    public void UI_Button_GameStart()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!_gpManager.Get_IsAuthenticated()) return;
#endif

        GameManager.instance.Load_Scene_Lobby();
    }
}
