using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public IntVariable durability;
    [Header("Body Parts")]
    public BodyPart head;
    public BodyPart body;
    public BodyPart LeftArm;
    public BodyPart RightArm;
    public BodyPart LeftLeg;
    public BodyPart RightLeg;

    #region Player Stat
    public int Atk { get; private set; }
    public int Def { get; private set; }
    public int Dex { get; private set; }
    public int Mana { get; private set; }
    public int Endurance { get; private set; }
    #endregion

    // for debugging
    public int forTest = 0;

    public void InitPlayer()
    {
        BodyRegenerationRate = 0;
        durability.runtimeValue = 100;
    }
    
    public void KillPlayer()
    {

    }

    #region Body decay
    private const int DECAY_RATE_EXPLORATION = 5;
    private const int DECAY_RATE_HOME = 3;
    public int BodyRegenerationRate {get; private set;}
    private int BodyDecayRate
    {
        get
        {
            if(GameManager.Inst.IsHome)
            {
                return DECAY_RATE_HOME - BodyRegenerationRate;
            }
            else
            {
                return DECAY_RATE_EXPLORATION - BodyRegenerationRate;
            }
        }
    }
    
    // 시간이 흘렀을 때 캐릭터의 변화에 대한 함수이다
    public void DecayBody()
    {
        durability.runtimeValue = durability.runtimeValue - BodyDecayRate;
    }
    #endregion

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        InitPlayer();
    }
    #endregion

}

