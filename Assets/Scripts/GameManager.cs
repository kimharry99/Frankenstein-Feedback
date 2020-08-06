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
    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        InitGame();
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
        HomeUIManager.Inst.panelExploration.SetActive(true);
        IsHome = false;
    }
    // Home으로 복귀하도록 하는 함수이다.
    public void ReturnHome()
    {
        IsHome = true;
    }

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

    public void DisassembleItem(/* some parameters */)
    {
        HomeUIManager.Inst.energy.runtimeValue += HomeUIManager.Inst.disassembleEnergy;
        HomeUIManager.Inst.UpdateEnergy();

        for (int i = 0; i < 9; i++)
        {
            if (HomeUIManager.Inst.slotDisassembleHolding[i].sprite == HomeUIManager.Inst.checkImage)
            {
                int slotNumber = HomeUIManager.Inst.itemIndex[i];
                StorageManager.Inst.DeleteItem(slotNumber, HomeUIManager.Inst.chest);
            }
        }

        HomeUIManager.Inst.UpdateDisassemble();

        OnTurnOver(1);
    }
}
