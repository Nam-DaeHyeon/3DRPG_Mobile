using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] List<int> _sellingList;
    List<Item> _goods = new List<Item>();

    [SerializeField] GameObject _InteractObj;

    private void Start()
    {
        StartCoroutine(IE_CreateList());
    }

    IEnumerator IE_CreateList()
    {
        Item item = null;

        for (int i = 0; i < _sellingList.Count; i++)
        {
            MySQLManager.instance.Create_Item(_sellingList[i]);
            yield return new WaitUntil(() => MySQLManager.instance.isReturn);
            item = MySQLManager.instance.ReturnItem;
            _goods.Add(item);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_sellingList.Count == 0) return;
        if (_goods.Count == 0) return;

        if(LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.Set_Available_NPC(_InteractObj.transform, _goods);
            Active_InteractiveObj();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.Set_Unavailable_NPC();
            Unactive_InteractiveObj();
            player.Set_Off_NPC_UI();
        }
    }

    public void Active_InteractiveObj()
    {
        _InteractObj.transform.parent = null;
        _InteractObj.SetActive(true);
    }

    public void Unactive_InteractiveObj()
    {
        _InteractObj.SetActive(false);
    }
}
