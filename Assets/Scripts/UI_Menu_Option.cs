using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu_Option : MonoBehaviour
{
    public void UI_GoLobby()
    {
        GameManager.instance.Load_Scene_Lobby();
    }

    public void UI_QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
