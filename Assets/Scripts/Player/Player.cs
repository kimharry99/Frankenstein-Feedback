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
    /// 플레이어의 신체를 교환한다.
    /// </summary>
    /// <param name="bodyPart">플레이어가 장착할 BodyPart</param>
    /// <param name="chestIndex">Player가 장착할 BodyPart의 chest Index</param>
    /// <returns>플레이어가 장착하고 있던 BodyPart</returns>
    public BodyPart ExchangePlayerBody(BodyPart bodyPart, int chestIndex)
    {
        BodyPart equipping;

        equipping = ExchangePlayerBodyObject(bodyPart, chestIndex);
        UpdatePlayerBodyPartSprite(bodyPart.bodyPartType);

        //switch (bodyPart.bodyPartType)
        //{
        //    case BodyPartType.Head:
        //        equipping = equippedBodyPart.Head;
        //        equippedBodyPart.Head = bodyPart;
        //        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = head.bodyPartSprite;
        //        break;
        //    case BodyPartType.Body:
        //        equipping = body;
        //        body = bodyPart;
        //        equippedBodyPart.Body = bodyPart;
        //        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = body.bodyPartSprite;
        //        break;
        //    case BodyPartType.LeftArm:
        //        equipping = leftArm;
        //        leftArm = bodyPart;
        //        equippedBodyPart.LeftArm = bodyPart;
        //        transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = leftArm.bodyPartSprite;
        //        break;
        //    case BodyPartType.RightArm:
        //        equipping = rightArm;
        //        rightArm = bodyPart;
        //        equippedBodyPart.RightArm = bodyPart;
        //        transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = rightArm.bodyPartSprite;
        //        break;
        //    case BodyPartType.LeftLeg:
        //        equipping = leftLeg;
        //        leftLeg = bodyPart;
        //        equippedBodyPart.LeftLeg = bodyPart;
        //        transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = leftLeg.bodyPartSprite;
        //        break;
        //    case BodyPartType.RightLeg:
        //        equipping = rightLeg;
        //        rightLeg = bodyPart;
        //        equippedBodyPart.RightLeg = bodyPart;
        //        transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sprite = rightLeg.bodyPartSprite;
        //        break;
        //    default:
        //        Debug.Log("wrong item Type");
        //        return null;
        //}

        return equipping;
    }

    private BodyPart ExchangePlayerBodyObject(BodyPart bodyPart, int chestIndex)
    {
        BodyPart equipping;
        equipping = equippedBodyPart.bodyParts[(int)bodyPart.bodyPartType];
        equippedBodyPart.bodyParts[(int)bodyPart.bodyPartType] = bodyPart;

        StorageManager.Inst.DeleteFromChest(chestIndex);
        StorageManager.Inst.AddItemToChest(equipping);
        return equipping;
    }

    /// <summary>
    /// bodyPartType에 해당하는 플레이어 신체 스프라이트를 업데이트한다.
    /// </summary>
    /// <param name="bodyPartType"></param>
    private void UpdatePlayerBodyPartSprite(BodyPartType bodyPartType)
    { 
        Player.Inst.transform.GetChild((int)bodyPartType).gameObject.GetComponent<SpriteRenderer>().sprite = equippedBodyPart.bodyParts[(int)bodyPartType].bodyPartSprite;
    }

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        InitPlayer();
    }
    #endregion

}

