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
    public CraftingTable craftingTable;
    public Research research;
    public IntVariable energy;
    //public IntVariable durabilityI;
    public FloatVariable durability;

    #region Unity Functions
    private void Start()
    {
        InitGame();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HomeScene")
        {
            IsHome = true;
            bodyAssembly = GameObject.Find("BodyAssembly").GetComponent<BodyAssembly>();
            bodyDisassembly = GameObject.Find("BodyDisassembly").GetComponent<BodyDisassembly>();
            craftingTable = GameObject.Find("CraftingTable").GetComponent<CraftingTable>();
            research = GameObject.Find("Research").GetComponent<Research>();
        }
        else
        {
            if (scene.name != "HomeScene")
                IsHome = false;
        }
    }
    #endregion

    /// <summary>
    /// 게임을 초기화 한다. 
    /// </summary>
    private void InitGame()
    {
        _time.SetTime(8);
        if (bodyDisassembly != null)
        {
            IsHome = true;
        }
        //for debugging
        //energy.value = 500;
    }

    // 탐사를 시작할 때 호출하는 함수이다.
    public void StartExploration()
    {
        SceneManager.LoadScene(1);
        
        HomeUIManager.Inst.panelHome.SetActive(false);
        ExplorationUIManager.Inst.panelExploration.SetActive(true);
        ExplorationManager.Inst.InitializeExploration();
        IsHome = false;
    }

    // Home으로 복귀하도록 하는 함수이다.
    public void ReturnHome()
    {
        SceneManager.LoadScene(0);

        ExplorationUIManager.Inst.panelExploration.SetActive(false);
        HomeUIManager.Inst.panelHome.SetActive(true);
        IsHome = true;
    }

    // turn 만큼의 턴을 소모한다.
    public void OnTurnOver(int turn)
    {
        SendTime(turn);
        Player.Inst.DecayBody(turn);
        GeneralUIManager.Inst.UpdateTextDurability();
        GeneralUIManager.Inst.UpdateTextTime();
        SendToSleep(_time.runtimeTime);
    }

    public void SendTime(int turn)
    {
        _time.SetTime(_time.runtimeTime+turn);
    }

    private float penaltyPerTime = 5.0f;
    /// <summary>
    /// 플레이어가 잠을 잘 때 호출, 
    /// </summary>'
    /// <param name="time">잠이 든 시각</param>
    public void SendToSleep(int time)
    {
        if (_time.isNight && IsHome)
        {
            Debug.Log("now sleeping...");
            int spendEnergy = 0;
            float regenDurability = 0.0f;
            //RegenBody(time, ref spendEnergy, ref regenDurability);
            spendEnergy = CalEnergyCostForRegen(time);
            regenDurability = CalRegenedDurability(time);
            if (_time.runtimeTime < 8)
                _time.SetTime(8);
            StartCoroutine(HomeUIManager.Inst.PutToSleep(time, penaltyPerTime, spendEnergy, regenDurability));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">잠이 든 시각</param>
    /// <param name="energy">수면에 사용한 에너지</param>
    /// <param name="durability">수면으로 회복한 내구도</param>
    public void RegenBody(int time, ref int spendEnergy, ref float regenDurability)
    {
        // 에너지를 통한 힐
        //int cost = Mathf.CeilToInt((100 - durabilityI.value) / 100.0f * GetEnergyCost(_time.runtimeDay));
        int cost = Mathf.CeilToInt((100 - durability.value) / 100.0f * GetEnergyCost(_time.runtimeDay));
        if (cost <= energy.value)
        {
            energy.value -= cost;
            spendEnergy = cost;
            //regenDurability = 100 - durabilityI.value;
            regenDurability = 100 - durability.value;
            //durabilityI.value = 100;
            durability.value = 100.0f;
            Debug.Log("내구도 전부 회복, 소모한 에너지 : " + cost);
        }
        else
        {
            cost = GetEnergyCost(_time.runtimeDay);
            Debug.Log("내구도 일부 회복\n소모한 에너지 : " + energy.value + "\n회복한 내구도 : " + 100.0f * energy.value / cost);
            spendEnergy = energy.value;
            regenDurability = 100.0f * energy.value / cost;
            //durabilityI.value += Mathf.CeilToInt(100.0f * energy.value / cost);
            durability.value += 100.0f * energy.value / cost;
            energy.value = 0;
        }
        // 비수면 페널티
        if(time > 0)
        {
            //durabilityI.value -= (int)(penaltyPerTime * time);
            durability.value -= penaltyPerTime * time;
            Debug.Log("비수면 페널티 : " + (int)(penaltyPerTime * time));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">수면 시작 시각</param>
    /// <returns>수면에 사용한 에너지</returns>
    private int CalEnergyCostForRegen(int time)
    {
        // 에너지를 통한 힐
        //int cost = Mathf.CeilToInt((100 - durabilityI.value) / 100.0f * GetEnergyCost(_time.runtimeDay));
        int cost = Mathf.CeilToInt((100 - durability.value) / 100.0f * GetEnergyCost(_time.runtimeDay));
        if (cost <= energy.value)
        {
            energy.value -= cost;
            return cost;
        }
        else
        {
            cost = energy.value;
            energy.value = 0;
            return cost;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">수면 시작 시각</param>
    /// <returns>수면으로 회복한 에너지</returns>
    private float CalRegenedDurability(int time)
    {
        // 에너지를 통한 힐
        //int cost = Mathf.CeilToInt((100 - durabilityI.value) / 100.0f * GetEnergyCost(_time.runtimeDay));
        int cost = Mathf.CeilToInt((100 - durability.value) / 100.0f * GetEnergyCost(_time.runtimeDay));
        float regenedDurability = 0.0f;
        if (cost <= energy.value)
        {
            regenedDurability = 100 - durability.value;
            //durabilityI.value = 100;
            durability.value = 100.0f;
        }
        else
        {
            cost = GetEnergyCost(_time.runtimeDay);
            Debug.Log("내구도 일부 회복\n소모한 에너지 : " + energy.value + "\n회복한 내구도 : " + 100.0f * energy.value / cost);
            regenedDurability = 100.0f * energy.value / cost;
            //durabilityI.value += Mathf.CeilToInt(100.0f * energy.value / cost);
            durability.value += regenedDurability;
            energy.value = 0;
        }
        // 비수면 페널티
        if (time > 0)
        {
            durability.value -= penaltyPerTime * time;
            Debug.Log("비수면 페널티 : " + (int)(penaltyPerTime * time));
        }
        return regenedDurability;
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

    public void GameOver()
    {
        Debug.Log("game over");
        Application.Quit();
    }
}
