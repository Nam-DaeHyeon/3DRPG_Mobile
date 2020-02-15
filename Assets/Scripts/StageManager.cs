using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Transform playerSpawnPos;
    public Transform monsterSpawnPos;

    public static StageManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SoundManager.instance.Play_BG_Main();
    }
}
