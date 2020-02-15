using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] ParticleSystem _selectEffect;
    [SerializeField] List<Transform> _startPosPool;
    [SerializeField] Transform _targetPos;

    [SerializeField] List<CharacterData> userData;
    [SerializeField] List<LobbyPlayer> _lobbyPlayer = new List<LobbyPlayer>();
    [SerializeField] LobbyPlayer _goPlayer;

    [Header("UI Element")]
    [SerializeField] Button _startButton;   //게임 시작 버튼 : 선택 캐릭터로 실행합니다.
    [SerializeField] Button _createButton;  //캐릭터 생성 버튼
    [SerializeField] Button _deleteButton;  //캐릭터 삭제 버튼
    [SerializeField] GameObject _selectPrefab;
    List<GameObject> _selectPool = new List<GameObject>();

    [Header("Canvas")]
    [SerializeField] GameObject _viewSELECT;
    [SerializeField] GameObject _viewCREATE;

    [Header("Create Part")]
    [SerializeField] Text _textViewCREATE_nickName;
    [SerializeField] Text _textViewCREATE_className;
    string crt_nickName = "nickname";
    string crt_className = "warrior";

    public Text myLog;
    
    public static LobbyManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SoundManager.instance.Play_BG_Lobby();

        Set_Initialize();
    }

    private void Set_Initialize()
    {
        StartCoroutine(IE_Set_Initialize());
    }

    private IEnumerator IE_Set_Initialize()
    { 
        _viewSELECT.SetActive(true);
        _viewCREATE.SetActive(false);

        if (_goPlayer == null)
        {
            _startButton.interactable = false;
            _createButton.interactable = true;
            _deleteButton.interactable = false;
        }

        myLog.text += "\n Initializing...";

        //초기화 : 기존 데이터 로드
        MySQLManager.instance.Load_Select_UserData(GameManager.instance.Get_LocalUserId());
        yield return new WaitUntil(() => MySQLManager.instance.Get_SelectEndCheck());
        userData = MySQLManager.instance.Get_Select_UserData();

        myLog.text += "\n" + GameManager.instance.Get_LocalUserId() + ": [" + userData.Count + "]";
        for (int i = 0; i < userData.Count; i++)
        {
            Create_SelectCharacterButton(i);

            //로드 데이터에 따른 캐릭터 가시화
            crt_className = userData[i].className;
            Create_CharacterObject(userData[i]);
        }
    }
    
    public void UI_Select_StartingPlayer(int playerIndex)
    {
        //중복 클릭 방지
        if (_goPlayer != null && _goPlayer.Equals(_lobbyPlayer[playerIndex])) return;

        //case 1 : AssetData
        //GameManager.instance.playerData = _resourcePlayer.srcPlayer[playerIndex];
        //case 2 : lobby player Class
        //GameManager.instance.playerData.nickName = _lobbyPlayer[playerIndex].nickName;
        //GameManager.instance.playerData.className = _lobbyPlayer[playerIndex].className;
        //GameManager.instance.playerData.level = _lobbyPlayer[playerIndex].level;
        //GameManager.instance.playerData.money = _lobbyPlayer[playerIndex].money;
        //case 3 : MYSQL loaded data
        GameManager.instance.playerData = userData[playerIndex];

        GameManager.instance.Save_SelectedCharacterModel(_lobbyPlayer[playerIndex].gameObject);

        _goPlayer = _lobbyPlayer[playerIndex];

        if (!_selectEffect.gameObject.activeSelf) _selectEffect.gameObject.SetActive(true);
        _selectEffect.transform.position = _goPlayer.transform.position;
        _selectEffect.transform.position += Vector3.up * 0.6f;      //위치 보정
        _selectEffect.transform.parent = _goPlayer.transform;
        _selectEffect.Play();

        _goPlayer.Move_toTargetPos(_targetPos.position);

        for(int i = 0; i < _lobbyPlayer.Count; i++)
        {
            if (_lobbyPlayer[i].Equals(_goPlayer)) continue;

            _lobbyPlayer[i].Move_toOriginPos();
        }

        //UI
        _startButton.interactable = true;
        _deleteButton.interactable = true;
    }

    public void UI_GameStart()
    {
        GameManager.instance.Load_Scene_Main();
    }
    
    public void UI_Create_Character()
    {
        crt_nickName = "";
        crt_className = "warrior";
        _textViewCREATE_nickName.text = crt_nickName;
        _textViewCREATE_className.text = crt_className;

        _viewCREATE.SetActive(true);
    }

    public void UI_Delete_Character()
    {
        //이펙트 종속성 제거
        _selectEffect.transform.parent = null;
        _selectEffect.gameObject.SetActive(false);

        //데이터 삭제
        int charIndex = _lobbyPlayer.IndexOf(_goPlayer);
        _lobbyPlayer.RemoveAt(charIndex);
        Destroy(_goPlayer.gameObject);

        GameObject selectBtn = _selectPool[charIndex];
        _selectPool.RemoveAt(charIndex);
        Destroy(selectBtn);

        MySQLManager.instance.Delete_CharacterData(userData[charIndex]);
        userData.RemoveAt(charIndex);

        //UI
        _startButton.interactable = false;
        _deleteButton.interactable = false;
    }

    public void UI_CreateView_Select(string className)
    {
        crt_className = className;
        _textViewCREATE_className.text = crt_className;
    }

    public void UI_CreateView_Decide()
    {
        StartCoroutine(IE_CreateView_Decide());
    }

    private IEnumerator IE_CreateView_Decide()
    {
        //userData 생성 
        CharacterData newData = new CharacterData();
        newData.Set_UserId(GameManager.instance.Get_LocalUserId());
        newData.nickName = _textViewCREATE_nickName.text;
        newData.className = crt_className;
        newData.level = 1;

        //능력치 배분 임시 2019.08.08
        newData.ab_str = 5;
        newData.ab_con = 5;
        newData.ab_int = 5;
        newData.ab_wis = 5;
        newData.ab_dex = 5;
        newData.point = 0;

        myLog.text += "\n Pressed Decide Button.";

        //MYSQL 생성
        MySQLManager.instance.Insert_CharacterData(newData);
        yield return new WaitUntil(() => MySQLManager.instance.Get_EndCheck());

        userData.Add(newData);
        Create_CharacterObject(newData);

        //메모리 갱신
        Create_SelectCharacterButton(userData.Count - 1);

        _viewCREATE.SetActive(false);
    }

    public void UI_CreateView_Back()
    {
        _viewCREATE.SetActive(false);
    }

    public LobbyPlayer Get_goPlayer() { return _goPlayer; }

    private void Create_CharacterObject(CharacterData data)
    {
        GameObject newObj = null;

        switch (crt_className.ToLower())
        {
            case "warrior":
                newObj = Instantiate(Resources.Load<GameObject>("Lobby/Warrior_Lobby"));
                break;
            case "magician":
                newObj = Instantiate(Resources.Load<GameObject>("Lobby/Magician_Lobby"));
                break;
        }

        if (_lobbyPlayer.Count < _startPosPool.Count)
        {
            newObj.transform.position = _startPosPool[_lobbyPlayer.Count].position;
            newObj.transform.rotation = _startPosPool[_lobbyPlayer.Count].rotation;
        }
        else
        {
            //시작 위치를 다 사용했다면 마지막 위치값을 사용한다.
            newObj.transform.position = _startPosPool[_startPosPool.Count - 1].position;
            newObj.transform.rotation = _startPosPool[_startPosPool.Count - 1].rotation;
        }
        
        if (newObj != null)
        {
            myLog.text += "\n newObj Pos : " + newObj.transform.position;
        }
        else
        {
            myLog.text += "\n newObj null => " + crt_className.ToLower();
        }

        LobbyPlayer getPlayer = newObj.GetComponent<LobbyPlayer>();
        getPlayer.nickName = data.nickName;
        getPlayer.className = data.className;
        getPlayer.level = data.level;
        getPlayer.money = data.money;
        getPlayer.ab_str = data.ab_str;
        getPlayer.ab_con = data.ab_con;
        getPlayer.ab_int = data.ab_int;
        getPlayer.ab_wis = data.ab_wis;
        getPlayer.ab_dex = data.ab_dex;
        getPlayer.point = data.point;

        _lobbyPlayer.Add(getPlayer);
    }

    private void Create_SelectCharacterButton(int dataIndex)
    {
        GameObject element = Instantiate(_selectPrefab, _selectPrefab.transform.parent);
        element.name = "Select " + _selectPool.Count;

        //NickName
        element.transform.GetChild(0).GetComponent<Text>().text = userData[dataIndex].nickName;
        //Level
        element.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = userData[dataIndex].level.ToString();
        //ClassName
        element.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = userData[dataIndex].className;
        //element.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _resourcePlayer.srcPlayer[_selectPool.Count].level.ToString();
        //element.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = _resourcePlayer.srcPlayer[_selectPool.Count].className.ToString();

        _selectPool.Add(element);

        element.SetActive(true);    //오브젝트 활성화
    }
}
