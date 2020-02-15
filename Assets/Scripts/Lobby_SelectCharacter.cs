using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_SelectCharacter : MonoBehaviour
{
    public void UI_Select_Character()
    {
        Transform parent = transform.parent;
        for(int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).Equals(transform))
            {
                //0번째 자식은 재생성을 위한 비활성화 자원(프리팹)이므로 -1의 보정을 해준다.
                LobbyManager.instance.UI_Select_StartingPlayer(i - 1);
                return;
            }
        }
    }
}
