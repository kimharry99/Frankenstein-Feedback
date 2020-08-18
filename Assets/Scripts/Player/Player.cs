using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public IntVariable durability;
    public Inventory inventory;
    [Header("Body Parts")]
    [SerializeField]
    private EquippedBodyPart _equippedBodyPart = null;

    #region Player Stat
    private int[] _raceAffinity = new int[6];
    public int GetRaceAffinity(Race race)
    {
        if (_raceAffinity[(int)race] == 0)
            return 1;
        return _raceAffinity[(int)race];
    }
    [Header("Status")]
    public Status toolStat;
    [SerializeField]
    private Status _bodyPartStat = null;
    public void ResetBodyAffinity()
    {
        _raceAffinity[(int)Race.All] = 1;
        for(int i=1;i<_raceAffinity.Length;i++)
        {
            _raceAffinity[i] = 0;
        }
    }
    public int Atk
    {
        get
        {
            return toolStat.atk + _bodyPartStat.atk;
        }
    }
    public int Def
    {
        get
        {
            return toolStat.def + _bodyPartStat.def;
        }
    }
    public int Dex
    {
        get
        {
            return toolStat.dex + _bodyPartStat.dex;
        }
    }
    public int Mana
    {
        get
        {
            return toolStat.mana + _bodyPartStat.mana;
        }
    }
    public int Endurance
    {
        get
        {
            return toolStat.endurance + _bodyPartStat.endurance;
        }
    }
    #endregion

    public void InitPlayer()
    {
        BodyRegenerationRate = 0;
        durability.value = 100;
        UpdateAllPlayerBodyStatus(_raceAffinity, _equippedBodyPart.bodyParts, _bodyPartStat);
        UpdateAllPlayerSprites();
    }
    
    public void KillPlayer()
    {

    }

    #region Body decay

    private int decayRateExploration = 5;
    private int decayRateHome = 3;
    public int BodyRegenerationRate {get; private set;}
    private int BodyDecayRate
    {
        get
        {
            if(GameManager.Inst.IsHome)
            {
                return decayRateHome - BodyRegenerationRate;
            }
            else
            {
                return decayRateExploration - BodyRegenerationRate;
            }
        }
    }
    
    /// <summary>
    /// turn만큼 신체의 부패를 진행한다.
    /// </summary>
    /// <param name="turn"></param>
    public void DecayBody(int turn)
    {
        durability.value = durability.value - BodyDecayRate * turn;
    }


    #endregion

    #region Change Player Body Methods

    /// <summary>
    /// 플레이어의 신체를 교환한다.
    /// </summary>
    /// <param name="bodyPart">플레이어가 장착할 BodyPart</param>
    /// <param name="chestIndex">Player가 장착할 BodyPart의 chest Index</param>
    /// <returns>플레이어가 장착하고 있던 BodyPart</returns>
    public void ExchangePlayerBody(BodyPart bodyPart, int chestIndex)
    {
        BodyPart returnedBodyPart;

        returnedBodyPart = ExchangePlayerBodyObject(_equippedBodyPart.bodyParts, bodyPart);
        StorageManager.Inst.DeleteFromChest(chestIndex);
        if(!StorageManager.Inst.AddItemToChest(returnedBodyPart))
        {
            ExchangePlayerBodyObject(_equippedBodyPart.bodyParts, returnedBodyPart);
            Debug.Log("신체 교환 실패, 창고에 아이템을 추가할 수 없습니다.");
            return;
        }
        ChangeAllPlayerBodyStatus(_bodyPartStat, _raceAffinity, bodyPart, returnedBodyPart);
        ChangePlayerBodyPartSprite(bodyPart.bodyPartType);

        #region legacy
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
        #endregion
        Debug.Log("장착된 신체 : " + bodyPart.itemName + "\n탈착된 신체 : " + returnedBodyPart.itemName);

        return;
    }

    private BodyPart ExchangePlayerBodyObject(BodyPart[] equippedBodyParts, BodyPart bodyPart)
    {
        BodyPart equipping;
        equipping = equippedBodyParts[(int)bodyPart.bodyPartType];
        equippedBodyParts[(int)bodyPart.bodyPartType] = bodyPart;
        return equipping;
    }

    /// <summary>
    /// bodyPartType에 해당하는 플레이어 신체 스프라이트를 업데이트한다.
    /// </summary>
    private void ChangePlayerBodyPartSprite(BodyPartType bodyPartType)
    { 
        Player.Inst.transform.GetChild((int)bodyPartType).gameObject.GetComponent<SpriteRenderer>().sprite = _equippedBodyPart.bodyParts[(int)bodyPartType].bodyPartSprite;
    }

    /// <summary>
    /// playerBodyPart와 returnedBodyPart의 신체교환에 대한 신체 스텟 변화를 적용한다. 
    /// </summary>
    private void ChangeAllPlayerBodyStatus(Status bodyPartStat, int[] raceAffinity, BodyPart playerBodyPart, BodyPart returnedBodyPart)
    {
        UpdateBodyStat(bodyPartStat, playerBodyPart, returnedBodyPart);
        UpdateBodyAffinity(raceAffinity, playerBodyPart, returnedBodyPart);
    }

    #endregion

    #region Update Player Methods

    /// <summary>
    /// 모든 신체의 Sprite를 Update한다.
    /// </summary>
    private void UpdateAllPlayerSprites()
    {
        for(int indexBody = 0; indexBody < _equippedBodyPart.bodyParts.Length; indexBody++)
        {
            ChangePlayerBodyPartSprite((BodyPartType)indexBody);
        }
    }

    // Player의 신체에 해당하는 스텟과 종족동화율을 업데이트한다.
    // StorageManager의 inventory status methods region을 참고
    /// <summary>
    /// Player의 모든 신체를 참조하여 bodyPartStatus와 raceAffinity를 Update한다.
    /// </summary>
    public void UpdateAllPlayerBodyStatus(int[] raceAffinity, BodyPart[] playerBodyParts, Status bodyPartStatus)
    {
        UpdateAllBodyAffinity(raceAffinity, playerBodyParts);
        UpdateAllBodyStat(bodyPartStatus, playerBodyParts);
    }

    /// <summary>
    /// 플레이어의 모든 신체(bodyParts)를 참조하여 종족 동화율을 Update한다.
    /// </summary>
    /// <param name="bodyParts">플레이어가 장착한 모든 신체의 배열</param>
    private void UpdateAllBodyAffinity(int[] raceAffinity, BodyPart[] bodyParts)
    {
        // TODO : bodyAffinity를 reset하는 메소드를 Player와 관련없도록 분리하자.
        Player.Inst.ResetBodyAffinity();
        for (int bodyIndex = 0; bodyIndex < bodyParts.Length; bodyIndex++)
        {
            BodyPart playerBodyPart = bodyParts[bodyIndex];
            UpdateBodyAffinity(raceAffinity, playerBodyPart);
        }
    }

    /// <summary>
    /// playerBodyPart의 종족동화율을 raceAffinity에 Update한다.
    /// </summary>
    /// <param name="returnedBodyPart"> 어떤 신체가 플레이어에게서 탈착되었을 경우 사용한다.</param>
    private void UpdateBodyAffinity(int[] raceAffinity, BodyPart playerBodyPart, BodyPart returnedBodyPart = null)
    {
        switch (playerBodyPart.bodyPartType)
        {
            case BodyPartType.Head:
            case BodyPartType.Body:
                raceAffinity[(int)playerBodyPart.race] += 3;
                if (returnedBodyPart != null)
                    raceAffinity[(int)returnedBodyPart.race] -= 3;
                break;
            case BodyPartType.LeftArm:
            case BodyPartType.RightArm:
            case BodyPartType.LeftLeg:
            case BodyPartType.RightLeg:
                raceAffinity[(int)playerBodyPart.race] += 1;
                if (returnedBodyPart != null)
                    raceAffinity[(int)returnedBodyPart.race] -= 1;
                break;
            default:
                Debug.Log("wrong item type");
                break;
        }
        if(returnedBodyPart != null)
        {
            if(playerBodyPart.race != returnedBodyPart.race)
            {
                StorageManager.Inst.UpdateToolStat();
            }
        }
    }

    /// <summary>
    /// 플레이어의 모든 신체를 참조하여 플레이어의 bodyPartStatus를 업데이트한다.
    /// </summary>
    private void UpdateAllBodyStat(Status bodyPartStatus, BodyPart[] playerBodyParts)
    {
        bodyPartStatus.ResetStatus();
        for (int bodyIndex = 0; bodyIndex < playerBodyParts.Length; bodyIndex++)
        {
            BodyPart playerBodyPart = playerBodyParts[bodyIndex];
            UpdateBodyStat(bodyPartStatus, playerBodyPart);
        }
    }

    /// <summary>
    /// playerBodyPart의 stat을 bodyPartStatus에 반영한다. 
    /// </summary>
    /// <param name="returnedBodyPart"> 어떤 신체가 플레이어에게서 탈착되었을 경우 사용한다.</param>
    private void UpdateBodyStat(Status bodyPartStatus, BodyPart playerBodyPart, BodyPart returnedBodyPart = null)
    {
        bodyPartStatus.atk += playerBodyPart.atk;
        bodyPartStatus.def += playerBodyPart.def;
        bodyPartStatus.dex += playerBodyPart.dex;
        bodyPartStatus.mana += playerBodyPart.mana;
        bodyPartStatus.endurance += playerBodyPart.endurance;

        if(returnedBodyPart != null)
        {
            bodyPartStatus.atk -= returnedBodyPart.atk;
            bodyPartStatus.def -= returnedBodyPart.def;
            bodyPartStatus.dex -= returnedBodyPart.dex;
            bodyPartStatus.mana -= returnedBodyPart.mana;
            bodyPartStatus.endurance -= returnedBodyPart.endurance;
        }
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

