using UnityEngine;
using System;
using System.Collections.Generic;
using System.Data;    //C#의 데이터 테이블 때문에 사용
using MySql.Data;     //MYSQL함수들을 불러오기 위해서 사용
using MySql.Data.MySqlClient;    //클라이언트 기능을사용하기 위해서 사용
using System.Collections;

public class MySQLManager : MonoBehaviour
{
    private WWW www = null;
    private bool endCheck = false;
    
    List<CharacterData> selectData = new List<CharacterData>();
    private bool selectCheck = false;

    public static MySQLManager instance;
    private void Awake()
    {
        instance = this;
    }

    public Item ReturnItem;
    public bool isReturn = false;
    public void Create_Item(int id)
    {
        isReturn = false;
        StartCoroutine(IE_Create_Item(id));
    }

    private IEnumerator IE_Create_Item(int id)
    {
        WWWForm wfMsg = WF_Select_Item_In_ItemDB(id);
        StartCoroutine(Send(wfMsg));
        yield return new WaitUntil(() => endCheck);
        ReturnItem = Load_Item_In_ItemDB(id);
        isReturn = true;
    }

    private WWWForm WF_Select_Item_In_ItemDB(int id)
    {
        WWWForm wf = new WWWForm();
        wf.AddField("select", "ItemDatabase");
        wf.AddField("ID", id);

        return wf;
    }

    private Item Load_Item_In_ItemDB(int id)
    {
        Item getItem = new Item();
        getItem.Set_Id(id);

        #region NO PHP
        /*
        sqlConnect(); //접속

        //DML

        //SELECT "나타낼 컬럼명" FROM "테이블명" WHERE "검색컬럼명" = ' "컴럼데이터검색" '
        string cmd = "SELECT " + "*" + " FROM " + Table_ItemDataBase + " WHERE " + "ID" + " = " + id;

        //END DML

        MySqlCommand dbcmd = new MySqlCommand(cmd, sqlconn); //명령어를 커맨드에 입력

        MySqlDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            getItem.Set_Id((int)reader["ID"]);
            getItem.Set_ItemName((string)reader["NAME"]);
            string type = (string)reader["TYPE"];
            getItem.Set_ItemType(type);
            switch (type.Trim())
            {
                case "무기":
                    getItem.Set_AdditiveATK((int)reader["ADD_ATK"]);
                    getItem.Set_AdditiveMATK((int)reader["ADD_MATK"]);
                    break;
                case "방어구":
                    getItem.Set_AdditiveDEF((int)reader["ADD_DEF"]);
                    getItem.Set_AdditiveMDEF((int)reader["ADD_MDEF"]);
                    break;
                case "악세사리":
                    getItem.Set_AdditiveSTR((int)reader["ADD_STR"]);
                    getItem.Set_AdditiveCON((int)reader["ADD_CON"]);
                    getItem.Set_AdditiveINT((int)reader["ADD_INT"]);
                    getItem.Set_AdditiveWIS((int)reader["ADD_WIS"]);
                    getItem.Set_AdditiveDEX((int)reader["ADD_DEX"]);
                    break;
                case "소모품":
                    getItem.Set_healHP((int)reader["HEAL_HP"]);
                    getItem.Set_healSP((int)reader["HEAL_SP"]);
                    break;
                case "기타":

                    break;
            }
            getItem.Set_Price((int)reader["PRICE"]);
        }

        dbcmd.Dispose();
        dbcmd = null;

        sqldisConnect();
        */
        #endregion

        string full = www.text;
        char[] tempArray = full.ToCharArray();
        string tempString = "";

        //Debug.Log("ItemLog : " + full);
        for (int i = 0; i < tempArray.Length; i++)
        {
            switch (tempArray[i])
            {
                case '?':
                    if (getItem.Get_ItemName() == null) { getItem.Set_ItemName(tempString); tempString = ""; continue; }
                    if (getItem.Get_ItemType() == null) { getItem.Set_ItemType(tempString); tempString = ""; continue; }
                    if (getItem.Get_healHP() == -1) { getItem.Set_healHP(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_healSP() == -1) { getItem.Set_healSP(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveATK() == -1) { getItem.Set_AdditiveATK(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveMATK() == -1) { getItem.Set_AdditiveMATK(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveDEF() == -1) { getItem.Set_AdditiveDEF(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveMDEF() == -1) { getItem.Set_AdditiveMDEF(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveSTR() == -1) { getItem.Set_AdditiveSTR(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveCON() == -1) { getItem.Set_AdditiveCON(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveINT() == -1) { getItem.Set_AdditiveINT(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveWIS() == -1) { getItem.Set_AdditiveWIS(int.Parse(tempString)); tempString = ""; continue; }
                    if (getItem.Get_AdditiveDEX() == -1) { getItem.Set_AdditiveDEX(int.Parse(tempString)); tempString = ""; continue; }
                    break;
                case '&':
                    if (getItem.Get_Price() == -1) { getItem.Set_Price(int.Parse(tempString)); tempString = ""; continue; }
                    break;
                default:
                    tempString += tempArray[i];
                    break;
            }
        }
        //Debug.Log(getItem.Get_ItemName() + ": 종류-" + getItem.Get_ItemType() + " 공-" + getItem.Get_AdditiveATK() + " 마공-" + getItem.Get_AdditiveMATK() + " 방-" + getItem.Get_AdditiveDEF() + " 마방-" + getItem.Get_AdditiveMDEF()
        //            + " str-" + getItem.Get_AdditiveSTR() + " con-" + getItem.Get_AdditiveCON() + " int-" + getItem.Get_AdditiveINT() + " wis-" + getItem.Get_AdditiveWIS() + " dex-" + getItem.Get_AdditiveDEX()+ " price-"+getItem.Get_Price());

        return getItem;
    }
   
    /// <summary> 추출된 www의 문자열을 분석하여 원하는 값을 리턴한다. </summary>
    public void Load_Select_UserData(string userId)
    {
        selectData.Clear();
        selectCheck = false;

        StartCoroutine(IE_Select_UserData(userId));
    }

    private IEnumerator IE_Select_UserData(string userId)
    {
        WWWForm wf = new WWWForm();
        wf.AddField("select", "MyCharacter");
        wf.AddField("ID", userId);
        StartCoroutine(Send(wf));
        yield return new WaitUntil(() => endCheck);
        if (LobbyManager.instance != null) LobbyManager.instance.myLog.text += "\n connected DB. \n " + www.text;

        string fullCols = www.text;
        char[] tempArray = fullCols.ToCharArray();
        int i = -1;

        CharacterData newData = null;
        string tempString = "";
        int tempItemId = 0;
        int tempCount = 0;

        do
        {
            i++;

            switch (tempArray[i])
            {
                case '%':   //캐릭터 초기화
                    newData = new CharacterData();
                    tempString = "";
                    tempItemId = 0;
                    tempCount = 0;
                    break;
                case '?':   //캐릭터 데이터 입력
                    //Debug.Log(tempString + "(" + i + ")");
                    if (newData.Get_UserId() == null) { newData.Set_UserId(tempString); tempString = ""; continue; }
                    if (newData.nickName == null) { newData.nickName = tempString; tempString = ""; continue; }
                    if (newData.className == null) { newData.className = tempString; tempString = ""; continue; }
                    if (newData.level == 0) { newData.level = int.Parse(tempString); tempString = ""; continue; }
                    if (newData.ab_str == 0) { newData.ab_str = int.Parse(tempString); tempString = ""; continue; }
                    if (newData.ab_con == 0) { newData.ab_con = int.Parse(tempString); tempString = ""; continue; }
                    if (newData.ab_int == 0) { newData.ab_int = int.Parse(tempString); tempString = ""; continue; }
                    if (newData.ab_wis == 0) { newData.ab_wis = int.Parse(tempString); tempString = ""; continue; }
                    if (newData.ab_dex == 0) { newData.ab_dex = int.Parse(tempString); tempString = ""; continue; }
                    if (newData.point == 0) { newData.point = int.Parse(tempString); tempString = ""; continue; }
                    if (newData.money == 0) { newData.money = int.Parse(tempString); tempString = ""; continue; }
                    break;
                case '/':   //캐릭터 인벤토리 아이템 id 저장
                    tempItemId = int.Parse(tempString);
                    tempString = "";
                    break;
                case '@':   //캐릭터 인벤토리 아이템 해당 id의 갯수 저장
                    tempCount = int.Parse(tempString);
                    tempString = "";

                    WWWForm wfMsg = WF_Select_Item_In_ItemDB(tempItemId);
                    StartCoroutine(Send(wfMsg));
                    yield return new WaitUntil(() => endCheck);
                    newData.Add_Item(Load_Item_In_ItemDB(tempItemId), tempCount);
                    break;
                case '&':   //캐릭터 파싱 완료
                    //Debug.Log(newData.Get_UserId() + " " + newData.nickName + " " + newData.className + " " + newData.level + " " + newData.ab_str + " " + newData.ab_con + " " + newData.ab_int + " " + newData.ab_wis + " " + newData.ab_dex + " " + newData.money);
                    selectData.Add(newData);
                    break;
                default:
                    tempString += tempArray[i];
                    break;
            }
        } while (!tempArray[i].Equals('#'));

        selectCheck = true;
    }

    public List<CharacterData> Get_Select_UserData() { return selectData; }
    public bool Get_SelectEndCheck() { return selectCheck; }
    public bool Get_EndCheck() { return endCheck; }

    public void Update_ItemCount(InventoryItemSet itemSet, CharacterData data)
    {
        #region NO PHP
        /*
        sqlConnect(); //접속

        //DML
        //UPDATE "테이블명" SET "컬럼명" = ' "세팅할데이터" ' WHERE "검색컬럼명" = ' "컴럼데이터검색" '
        //string cmd = "SELECT " + "*" + " FROM " + Table_UserData_Inventory + userId + "_" + data.nickName;
        string cmd = "UPDATE " + Table_UserData_Inventory + userId + "_" + data.nickName + " SET COUNT = " + itemSet.itemCount + " WHERE ID = " + itemSet.item.Get_Id();
        //END DML

        MySqlCommand dbcmd = new MySqlCommand(cmd, sqlconn); //명령어를 커맨드에 입력
        dbcmd.ExecuteNonQuery();

        sqldisConnect();
        */
        #endregion

        endCheck = false;

        WWWForm wf = new WWWForm();
        wf.AddField("modify", "UpdateInventory");
        wf.AddField("ID", data.Get_UserId());
        wf.AddField("NICKNAME", data.nickName);

        wf.AddField("ITEM_ID", itemSet.item.Get_Id());
        wf.AddField("ITEM_COUNT", itemSet.itemCount);

        StartCoroutine(Send(wf));
    }

    public void Insert_CharacterData(CharacterData data)
    {
        #region NO PHP
        /*
        sqlConnect(); //접속

        //DML
        //INSERT INTO "테이블명" VALUES(' "필드명1" ', ' "필드명2" ')
        //UPDATE "테이블명" SET "컬럼명" = ' "세팅할데이터" ' WHERE "검색컬럼명" = ' "컴럼데이터검색" '
        //string cmd = "SELECT " + "*" + " FROM " + Table_UserData_Inventory + userId + "_" + data.nickName;
        //string cmd = "UPDATE " + Table_UserData_Inventory + userId + "_" + data.nickName + " SET COUNT = " + itemSet.itemCount + " WHERE ID = " + itemSet.item.Get_Id();
        string cmd = "INSERT INTO " + Table_UserData_Basic + data.Get_UserId()
                     + " VALUES('" + data.nickName + "', '" + data.className + "', '1', '"
                     + data.ab_str + "', '" + data.ab_con + "', '" + data.ab_int + "', '" + data.ab_wis + "', '" + data.ab_dex + "', '"
                     + data.money + "')";
        //END DML

        MySqlCommand dbcmd = new MySqlCommand(cmd, sqlconn); //명령어를 커맨드에 입력
        dbcmd.ExecuteNonQuery();

        CreateDB_CharacterInventory(data);

        sqldisConnect();
        */
        #endregion

        endCheck = false;

        WWWForm wf = new WWWForm();
        wf.AddField("modify", "InsertCharacter");
        wf.AddField("ID", data.Get_UserId());
        wf.AddField("NICKNAME", data.nickName);
        wf.AddField("CLASSNAME", data.className);
        wf.AddField("LEVEL", data.level);
        wf.AddField("_STR", data.ab_str);
        wf.AddField("_CON", data.ab_con);
        wf.AddField("_INT", data.ab_int);
        wf.AddField("_WIS", data.ab_wis);
        wf.AddField("_DEX", data.ab_dex);
        wf.AddField("POINT", data.point);
        wf.AddField("MONEY", data.money);

        StartCoroutine(Send(wf));
    }

    public void Delete_CharacterData(CharacterData data)
    {
        #region NO PHP
        /*
        //sqlConnect(); //접속
        
        //DML
        //DELETE FROM "테이블명" WHERE 조건
        string cmd = "DELETE FROM " + Table_UserData_Basic + data.Get_UserId() + " WHERE NICKNAME = '" + data.nickName +"'";
        //END DML

        MySqlCommand dbcmd = new MySqlCommand(cmd, sqlconn); //명령어를 커맨드에 입력
        dbcmd.ExecuteNonQuery();

        DeleteDB_CharacterInventory(data);
        
        //sqldisConnect();
        */
        #endregion

        WWWForm wf = new WWWForm();
        wf.AddField("modify", "DeleteCharacter");
        wf.AddField("ID", data.Get_UserId());
        wf.AddField("NICKNAME", data.nickName);

        StartCoroutine(Send(wf));
    }

    public void Update_PlayingCharacterData()
    {
        endCheck = false;

        CharacterData data = GameManager.instance.playerData;

        WWWForm wf = new WWWForm();
        wf.AddField("modify", "UpdateCharacter");
        wf.AddField("ID", data.Get_UserId());
        wf.AddField("NICKNAME", data.nickName);
        wf.AddField("CLASSNAME", data.className);
        wf.AddField("LEVEL", data.level);
        wf.AddField("_STR", data.ab_str);
        wf.AddField("_CON", data.ab_con);
        wf.AddField("_INT", data.ab_int);
        wf.AddField("_WIS", data.ab_wis);
        wf.AddField("_DEX", data.ab_dex);
        wf.AddField("POINT", data.point);
        wf.AddField("MONEY", data.money);
        
        StartCoroutine(Send(wf));
    }

    public IEnumerator Send(WWWForm wf)
    {
        endCheck = false;

        www = new WWW("http://yumsoya.dothome.co.kr/connectDB.php", wf);
        yield return www;
        //Debug.Log(www.text);
        endCheck = true;
    }
}
