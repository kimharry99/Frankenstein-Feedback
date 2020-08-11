using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public FloatVariable durability;
    public Inventory inventory;
    [Header("Body Parts")]
    public BodyPart head;
    public BodyPart body;
    public BodyPart LeftArm;
    public BodyPart RightArm;
    public BodyPart LeftLeg;
    public BodyPart RightLeg;

    #region Player Stat
    //public IntVariable atk;
    //public IntVariable def;
    //public IntVariable dex;
    //public IntVariable mana;
    //public IntVariable endurance;
    [Header("Status")]
    public Status toolStat;
    public Status bodyPartStat;
    public int Atk
    {
        get
        {
            return toolStat.atk + bodyPartStat.atk;
        }
    }
    public int Def
    {
        get
        {
            return toolStat.def + bodyPartStat.def;
        }
    }
    public int Dex
    {
        get
        {
            return toolStat.dex + bodyPartStat.dex;
        }
    }
    public int Mana
    {
        get
        {
            return toolStat.mana + bodyPartStat.mana;
        }
    }
    public int Endurance
    {
        get
        {
            return toolStat.endurance + bodyPartStat.endurance;
        }
    }
    #endregion

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

