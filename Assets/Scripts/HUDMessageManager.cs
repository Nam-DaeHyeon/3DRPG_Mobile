using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMessageManager : MonoBehaviour
{
    [Header("TextMesh")]
    [SerializeField] GameObject tmPrefab;
    [SerializeField] List<GameObject> tmPool;
    [SerializeField] int tmPoolCount;

    public static HUDMessageManager instance;
    private void Awake()
    {
        instance = this;

        for(int i = 0; i<tmPoolCount; i++)
        {
            tmPool.Add(Instantiate(tmPrefab));
        }
    }

    public void Load_TextMessage(Vector3 loadPos, string msg)
    {
        TextMessage temp = null;

        for(int i = 0; i < tmPool.Count; i++)
        {
            if (!tmPool[i].activeSelf) temp = tmPool[i].GetComponent<TextMessage>();
        }
        if(temp == null)
        {
            GameObject obj = Instantiate(tmPrefab);
            tmPool.Add(obj);

            temp = obj.GetComponent<TextMessage>();
        }

        temp.transform.position = loadPos;
        temp.gameObject.SetActive(true);
        temp._textMesh.text = msg;
        StartCoroutine(IE_Hide_TextMessgaeObj(temp.gameObject));
    }

    IEnumerator IE_Hide_TextMessgaeObj(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);
    }
}
