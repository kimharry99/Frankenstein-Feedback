﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    private Character _character = Character.Inst;
    // 시간을 time만큼 보내기 위해서 호출된다.
    public void SendTime(int time)
    {
        _character.SendTime(time);
        Time = Time + time;
        HomeUIManager.Inst.UpdateTextTime(Day, Time);
    }
}
