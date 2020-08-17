using System.Collections;
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

    /// <summary>
    /// 게임을 초기화 한다. 
    /// </summary>
    private void InitGame()
    {
        time.SetTime(8);
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

    // turn 만큼의 턴을 소모한다.
    public void OnTurnOver(int turn)
    {
        SendTime(turn);
        Player.Inst.DecayBody(turn);
        GeneralUIManager.Inst.UpdateTextDurability();
        GeneralUIManager.Inst.UpdateTextTime();

        if(time.isNight && IsHome)
        {
            SendToSleep();
        }
    }
    public void SendTime(int turn)
    {
        time.SetTime(time.runtimeTime+turn);
    }

    /// <summary>
    /// 플레이어가 잠을 잘 때 호출, 
    /// </summary>
    public void SendToSleep()
    {
        RegenBody();
        HomeUIManager.Inst.NoticeEnergyChange();
    }

    public void RegenBody()
    {

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
    public void Foo(int a)
    {
        switch (a)
        {
            default:
                Debug.Log("Default");
                break;
            case 0:
                Debug.Log("0");
                break;
            case 1:
                Debug.Log("1");
                break;
        }
    }
}
