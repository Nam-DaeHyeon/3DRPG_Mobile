using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPosYEditor : MonoBehaviour
{
    Transform _controllerTr;

    Vector3 prevPos;
    Vector3 currPos;

    Vector3 dir;
    Vector3 dirSrc;
    Vector3 dirDest;
    float height = 500;
    [SerializeField] LayerMask _rayMask;
    RaycastHit _hit;

    private void OnEnable()
    {
        StartCoroutine(IE_DetectObjHeight());    
    }

    IEnumerator IE_DetectObjHeight()
    {
        while(true)
        {
            currPos = transform.position;

            dir = currPos - prevPos;

            dirSrc = transform.position + dir + Vector3.up * height;
            dirDest = (new Vector3(transform.position.x + dir.x, _controllerTr.position.y, transform.position.z + dir.z)) - dirSrc;

            Debug.DrawRay(dirSrc, dirDest, Color.red);
            if (Physics.Raycast(dirSrc, dirDest, out _hit, height * 1.5f, _rayMask))
            {
                transform.position = new Vector3(transform.position.x, _hit.point.y, transform.position.z);
            }
            else transform.position = new Vector3(transform.position.x, _controllerTr.position.y, transform.position.z);

            prevPos = currPos;

            yield return null;
        }
    }

    public void Set_Controller(Transform controller)
    {
        _controllerTr = controller;
    }
}
