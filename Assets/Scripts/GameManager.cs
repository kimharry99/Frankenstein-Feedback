using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int Day { get; private set; }
    [SerializeField]
    private Time _time;
    public bool IsHome { get; private set; }
    public BodyAssembly bodyAssembly;
    public BodyDisassembly bodyDisassembly;
    public IntVariable energy;
    public FloatVariable durability;

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
        _time.SetTime(8);
        IsHome = true;

        //for debugging
        energy.value = 500;
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

        if(_time.isNight && IsHome)
        {
            SendToSleep(_time.runtimeTime);
        }
    }
    public void SendTime(int turn)
    {
        _time.SetTime(_time.runtimeTime+turn);
    }

    /// <summary>
    /// 플레이어가 잠을 잘 때 호출, 
    /// </summary>'
    /// <param name="time">잠이 든 시각</param>
    public void SendToSleep(int time)
    {
        Debug.Log("now sleeping...");
        RegenBody(time);
        if(_time.runtimeTime < 8)
            _time.SetTime(8);
        GeneralUIManager.Inst.UpdateTextDurability();
        GeneralUIManager.Inst.UpdateTextTime();
        GeneralUIManager.Inst.UpdateEnergy();
        HomeUIManager.Inst.NoticeEnergyChange();
    }

    public void RegenBody(int time)
    {
        // 에너지를 통한 힐
        int cost = GetEnergyCost(_time.runtimeDay);
        if (cost <= energy.value)
        {
            energy.value -= cost;
            durability.value = 100.0f;
            Debug.Log("내구도 전부 회복, 소모한 에너지 : " + cost);
        }
        else
        {
            Debug.Log("내구도 일부 회복\n소모한 에너지 : " + energy.value+"\n회복한 내구도 : "+ 100.0f * energy.value / cost);
            durability.value += 100.0f * energy.value / cost;
            energy.value = 0;
        }
        // 비수면 페널티
        if(time > 1)
        {
            durability.value -= 5.0f * (time - 1);
            Debug.Log("비수면 페널티 : " + 5.0f * time);
        }
    }
    private int GetEnergyCost(int day)
    {
        return 5000 + 200 * (day - 1);
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
