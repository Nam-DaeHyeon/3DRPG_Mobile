using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CharacterData playerData = new CharacterData();

    public GameObject spellPoolObj;
    public GameObject itemPoolObj;

    [Header("Character Model Data")]
    public GameObject selectedCharacterModel;

    public static GameManager s_instance;
    public static GameManager instance
    {
        get
        {
            if (!s_instance)
            {
                s_instance = FindObjectOfType(typeof(GameManager)) as GameManager;
                if (!s_instance)
                {
                    Debug.LogError("GameManager s_instance null");
                    return null;
                }
            }

            return s_instance;
        }
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;

            DontDestroyOnLoad(this);

        }
        else if (this != s_instance)
        {
            Destroy(gameObject);
        }

        StartCoroutine(IE_Device_InteractButton());
    }

    public void Save_SelectedCharacterModel(GameObject sample)
    {
        //selectedCharacterModel = charModels[charIndex];
        selectedCharacterModel = sample;
    }

    public void Load_Scene_Title()
    {
        //SceneManager.LoadSceneAsync("Title");
        LoadingSceneManager.LoadScene("Title");
    }

    public void Load_Scene_Lobby()
    {
        //SceneManager.LoadSceneAsync("Lobby");
        LoadingSceneManager.LoadScene("Lobby");
    }

    public void Load_Scene_Main()
    {
        GameObject newOne = Instantiate(selectedCharacterModel);
        newOne.transform.parent = transform;
        newOne.transform.position = Vector3.forward * 1000f;

        Destroy(newOne.GetComponent<CharacterController>());
        Destroy(newOne.GetComponent<LobbyPlayer>());
        if (newOne.transform.childCount > 2)
        {
            Transform effect = newOne.transform.GetChild(2);
            effect.transform.parent = null;
            Destroy(effect.gameObject);
        }

        selectedCharacterModel = newOne;

        //SceneManager.LoadSceneAsync("Main");
        LoadingSceneManager.LoadScene("Main");
    }

    public string Get_LocalUserId()
    {
#if UNITY_EDITOR
        return "sample";
#elif UNITY_ANDROID && !UNITY_EDITOR
        return Social.localUser.id;
#endif
    }

    private IEnumerator IE_Device_InteractButton()
    {
        while (true)
        {
#if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (SceneManager.GetActiveScene().name)
                {
                    case "Title": //Title Scene
                        //case 게임 종료
                        Application.Quit();
                        break;
                    case "Lobby": //Lobby Scene
                        //case 게임 종료
                        Application.Quit();
                        break;
                    case "Main": //Main Scene
                        //case 로비 화면으로 이동
                        Load_Scene_Lobby();
                        break;
                }
            }
#else
            yield break;
#endif

            yield return null;
        }
    }

    public void Save_UserData()
    {

    }

    private void OnApplicationQuit()
    {
        
    }
}
