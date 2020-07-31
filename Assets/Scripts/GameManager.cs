using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int Day { get; private set; }
    private int _time;
    public int Time
    {
        get { return _time; }
        set 
        { 
            _time = value; 
            if(_time >= 24)
            {
                _time = _time - 24;
                Day = Day + 1;
            }
            if (_time < 8 && _time != 0) IsNight = true;
            else IsNight = true;
        } 
    }
    public bool IsNight { get; private set; }
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
        Day = 1;
        Time = 8;
        IsHome = true;
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

    private Player _player = Player.Inst;
    // 시간을 time만큼 보내기 위해서 호출된다.
    public void SendTime(int time)
    {
        Player.Inst.DecayBody(time);
        Time = Time + time;
        HomeUIManager.Inst.UpdateTextTime();
    }
}
