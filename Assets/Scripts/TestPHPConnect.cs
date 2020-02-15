using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPHPConnect : MonoBehaviour
{
    /*
    WWW www;
    public bool isDownloaded = false;

    private void Start()
    {
        StartCoroutine(Send());
    }

    private void Update()
    {
        if (!isDownloaded) Debug.Log("Progress : " + www.progress * 100 + "%");
        else Debug.Log(www.text);
    }

    IEnumerator Send()
    {
        isDownloaded = false;

        www = new WWW("http://yumsoya.dothome.co.kr/phpSample.php?select=show");
        yield return www;
        if (www.isDone) isDownloaded = true;
    }
    */
    
    public string nickname;
    public string classname;
    WWW www;

    private void OnEnable()
    {
        //submit();
        selectTest();
    }

    public void submit()
    {
        WWWForm w = new WWWForm();
        w.AddField("select", "submit");
        w.AddField("NICKNAME", nickname);
        w.AddField("CLASSNAME", classname);
        
        StartCoroutine(Send(w));
    }

    public void selectTest()
    {
        WWWForm w = new WWWForm();
        w.AddField("select", "MyCharacter");
        w.AddField("ID", "sample");
        StartCoroutine(Send(w));
    }

    IEnumerator Send(WWWForm wF)
    {
        Debug.Log("Sending...");
        www = new WWW("http://yumsoya.dothome.co.kr/connectDB.php", wF);
        //www = new WWW("http://yumsoya.dothome.co.kr/interfaceUserData.php", wF);
        //www = new WWW("http://yumsoya.dothome.co.kr/phpSample.php", wF);
        //www = new WWW("http://yumsoya.dothome.co.kr/index.php", wF);
        yield return www;
        
        Debug.Log("Finished Sending.");
        Debug.Log(www.text);
    }


}
