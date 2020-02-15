using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    Vector3 _initCamPos;

    IEnumerator IE_Track_Camera()
    {
        _initCamPos = _trackingCam.transform.position;

        while (p_State.Equals(PlayerState.NONE))
        {
            _trackingCam.transform.position = transform.position + _initCamPos;

            yield return null;
        }
    }

}
