using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public FloatVariable durability;
    public int[] raceAffinity = new int[6];
    public Inventory inventory;
    [Header("Body Parts")]
    public EquippedBodyPart equippedBodyPart;
    public BodyPart head;
    public BodyPart body;
    public BodyPart leftArm;
    public BodyPart rightArm;
    public BodyPart leftLeg;
    public BodyPart rightLeg;
    #region Player Stat
    //public IntVariable atk;
    //public IntVariable def;
    //public IntVariable dex;
    //public IntVariable mana;
    //public IntVariable endurance;
    [Header("Status")]
    public Status toolStat;
    public Status bodyPartStat;
    public void ResetBodyAffinity()
    {
        raceAffinity[(int)Race.All] = 1;
        for(int i=1;i<raceAffinity.Length;i++)
        {
            raceAffinity[i] = 0;
        }
    }
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
        durability.value = 100;
        ResetBodyAffinity();
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
        durability.value = durability.value - BodyDecayRate;
    }

    #endregion

    /// <summary>
    /// 플레이어의 신체 스프라이트를 업데이트한다.
    /// </summary>
    public BodyPart UpdateCharacterBody(BodyPart bodyPart, int index)
    {
        BodyPart equiping;

        switch (bodyPart.bodyPartType)
        {
            case BodyPartType.Head:
                equiping = head;
                head = bodyPart;
                equippedBodyPart.Head = bodyPart;
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = head.bodyPartSprite;
                break;
            case BodyPartType.Body:
                equiping = body;
                body = bodyPart;
                equippedBodyPart.Body = bodyPart;
                transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = body.bodyPartSprite;
                break;
            case BodyPartType.LeftArm:
                equiping = leftArm;
                leftArm = bodyPart;
                equippedBodyPart.LeftArm = bodyPart;
                transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = leftArm.bodyPartSprite;
                break;
            case BodyPartType.RightArm:
                equiping = rightArm;
                rightArm = bodyPart;
                equippedBodyPart.RightArm = bodyPart;
                transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = rightArm.bodyPartSprite;
                break;
            case BodyPartType.LeftLeg:
                equiping = leftLeg;
                leftLeg = bodyPart;
                equippedBodyPart.LeftLeg = bodyPart;
                transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = leftLeg.bodyPartSprite;
                break;
            case BodyPartType.RightLeg:
                equiping = rightLeg;
                rightLeg = bodyPart;
                equippedBodyPart.RightLeg = bodyPart;
                transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sprite = rightLeg.bodyPartSprite;
                break;
            default:
                Debug.Log("wrong item Type");
                return null;
        }

        StorageManager.Inst.DeleteFromChest(index);
        StorageManager.Inst.AddItemToChest(equiping);

        return equiping;
    }

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        InitPlayer();
    }
    #endregion

}

