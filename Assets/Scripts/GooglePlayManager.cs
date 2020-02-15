using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayManager : MonoBehaviour
{
    [SerializeField] Text myLog;

    [SerializeField] Text userName;
    [SerializeField] RawImage userImage;

    private bool bWaitingForAuth = false;

    private void Awake()
    {
        myLog.text = "Ready...";

        //구글게임서비스 활성화(초기화)
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        Set_AutoLogIn();
    }

    //자동 로그인
    public void Set_AutoLogIn()
    {
        myLog.text = "...";

        if (bWaitingForAuth) return;

        //구글 로그인이 되어있지 않다면..
        if(!Social.localUser.authenticated)
        {
            myLog.text = "Authenticating...";
            bWaitingForAuth = false;

            //로그인 인증 처리과정 (콜백함수)
            Social.localUser.Authenticate(AuthenticateCallBack);
        }
    }

    private void Set_ManualLogIn()
    {
        if(Social.localUser.authenticated)
        {
            myLog.text = "name : " + Social.localUser.userName + "\n";
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if(success)
                {
                    myLog.text = "name : " + Social.localUser.userName + "\n";

                    userName.text = Social.localUser.userName;
                    StartCoroutine(IE_Load_UserPicture());
                }
                else
                {
                    myLog.text = "LogIn Fail\n";
                }
            });
        }
    }

    private void Set_ManualLogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        myLog.text = "LogOut...";

        userName.text = "";
        userImage.texture = null;
    }

    public void UI_Button_LogIn()
    {
        Set_ManualLogIn();
    }

    public void UI_Button_LogOut()
    {
        Set_ManualLogOut();
    }

    //인증 콜백
    private void AuthenticateCallBack(bool success)
    {
        myLog.text = "Loading...";
        
        if(success)
        {
            myLog.text = "Welcome " + Social.localUser.userName + " : " + Social.localUser.id + "\n";

            userName.text = Social.localUser.userName;
            StartCoroutine(IE_Load_UserPicture());
        }
        else
        {
            myLog.text = "Login Fail\n";
        }
    }

    IEnumerator IE_Load_UserPicture()
    {
        //myLog.text = "Image Loading...";

        Texture2D pic = Social.localUser.image;

        while(pic == null)
        {
            pic = Social.localUser.image;
            yield return null;
        }

        userImage.texture = pic;
        //myLog.text = "Image Created";
    }

    public bool Get_IsAuthenticated()
    {
        if (!Social.localUser.authenticated) myLog.text = "Please Log In";

        return Social.localUser.authenticated;
    }
}
