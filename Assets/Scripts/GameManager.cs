﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int Day { get; private set; }
    public Time time;
    public bool IsHome { get; private set; }
    public BodyAssembly bodyAssembly;
    public BodyDisassembly bodyDisassembly;

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitGame();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HomeScene")
        {
            bodyAssembly = GameObject.Find("BodyAssembly").GetComponent<BodyAssembly>();
            bodyDisassembly = GameObject.Find("BodyDisassembly").GetComponent<BodyDisassembly>();
        }
    }
    #endregion

    // 게임을 초기화 한다.
    private void InitGame()
    {
        //time.day = 1;
        //time.SetTime(8);
        onTurnOver.Invoke();
    }

    // 탐사를 시작할 때 호출하는 함수이다.
    public void StartExploration()
    {
        SceneManager.LoadScene(1);
        
        HomeUIManager.Inst.panelHome.SetActive(false);
        ExplorationUIManager.Inst.panelExploration.SetActive(true);
        IsHome = false;
    }
    // Home으로 복귀하도록 하는 함수이다.
    public void ReturnHome()
    {
        SceneManager.LoadScene(0);
        IsHome = true;
    }

    // TODO : onTurnOver를 Script내에서 구현
    public UnityEvent onTurnOver;
    // turn 만큼의 턴을 소모한다.
    public void OnTurnOver(int turn)
    {
        for(int i=0;i<turn;i++)
        {
            onTurnOver.Invoke();
        }
    }
    public void SendTime()
    {
        time.SetTime(time.runtimeTime+1);
    }

    //public void DisassembleItem(/* some parameters */)
    //{
    //    int count = 0;

    //    for (int i = 0; i < 30; i++)
    //    {
    //        if (HomeUIManager.Inst.imageCheck[i].activeSelf == true)
    //        {
    //            count++;
    //            int slotNumber = HomeUIManager.Inst.indexHoldingChest[i];
    //            // TODO : dest를 StorageManager로 수정
    //            StorageManager.Inst.DeleteFromChest(slotNumber);
    //        }
    //    }

    //    if (count == 0)
    //    {
    //        HomeUIManager.Inst.panelNotice.SetActive(true);
    //        HomeUIManager.Inst.textNotice.text = "분해할 사체를 선택하세요.";
    //        return;
    //    }

    //    GeneralUIManager.Inst.energy.value += HomeUIManager.Inst.disassembleEnergy;
    //    GeneralUIManager.Inst.UpdateEnergy();

    //    HomeUIManager.Inst.UpdateDisassemble();

    //    OnTurnOver(1);
    //}

    // for debugging
    public void Foo()
    {
        int index = StorageManager.Inst.GetIndexTable(Type.BodyPart, 0);
    }
}
