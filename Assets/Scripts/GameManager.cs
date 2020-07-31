using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int Day { get; private set; }
    private int _time;
    public int Time
    {
        get { return _time; }
        set { _time = value; }
    }
    public bool IsNight { get; private set; }

    private Character _character = Character.inst;
    public void SendTime(int time)
    {
        _character.SendTime(time);
    }
}
