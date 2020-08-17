using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 진웅 TODO : 클래스 구현, 변수와 메소드의 추가는 자유롭게 해도 되지만 삭제는 하지않고 구현했으면 좋겠습니다
public class BodyAssembly : MonoBehaviour
{
    private BodyPart[] _holdingBodyParts = new BodyPart[Chest.CAPACITY];
    // holding된 BodyPart와 chest의 slotNumber간의 index
    private int[] _index = new int[Chest.CAPACITY];

    private StorageManager _sm = StorageManager.Inst;
    // 창고의 BodyPart를 로드해서 holdingbodyParts배열에 저장한다. index 또한 업데이트한다.
    // StorageManager.GetIndexTable()을 사용하여 구현
    public void HoldBodyPartsFromChest()
    {
        for (int i = 0; i < Chest.CAPACITY; i++)
        {
            _index[i] = StorageManager.Inst.GetIndexTable(Type.BodyPart, i);
            if (_index[i] != -1)
            {
                _holdingBodyParts[i] = (BodyPart)HomeUIManager.Inst.chest.slotItem[_index[i]];
                continue;
            }
            _holdingBodyParts[i] = null;
        }
    }

    private int _selectedSlotOfChest;
    private BodyPart _selectedBodyPart;
    // selectedBodyPart를 업데이트 한다.
    public void SelectBodyPart(int holdingSlotNumber)
    {
        _selectedSlotOfChest = _index[holdingSlotNumber];
        _selectedBodyPart = _holdingBodyParts[holdingSlotNumber];
    }

    [SerializeField]
    private IntVariable energy;
    // 신체 조립을 실행한다.
    // StorageManager의 아이템 Add함수, Delete함수를 사용하여 구현
    public void AssemleBody()
    {
        // 선택된 신체가 창고에서 이동되어 플레이어에게 장착된다. 
        // Player가 장착하고 있던 신체는 창고로 이동한다.
        // 에너지를 소비한다.
        BodyPart returnedBodyPart = Player.Inst.UpdateCharacterBody(_selectedBodyPart, _selectedSlotOfChest);
        SpendEnergy(100);
        HomeUIManager.Inst.UpdateBodyAssemblyHoldingImages();

        //UpdateBodyStat(returnedBodyPart);
        UpdatePlayerBodyStatus(Player.Inst.raceAffinity, Player.Inst.equippedBodyPart.bodyParts, _bodyPartStatus);
        GameManager.Inst.OnTurnOver(1);
        GeneralUIManager.Inst.UpdateEnergy();
    }

    private void SpendEnergy(int energyCost)
    {
        // energy UI는 구현하지 않는다.
        energy.value -= energyCost;
    }

    [SerializeField]
    private Status _bodyPartStatus = null;
    // Player의 신체에 해당하는 스텟을 업데이트한다.
    // StorageManager의 inventory status methods region을 참고
    public void UpdatePlayerBodyStatus(int[] raceAffinity, BodyPart[] bodyParts, Status bodyPartStatus)
    {
        UpdatePlayerBodyAffinity(raceAffinity, bodyParts);
        UpdatePlayerBodyStat(_bodyPartStatus, bodyParts);
    }

    /// <summary>
    /// 플레이어의 모든 신체(bodyParts)를 참조하여 종족 동화율을 Update한다.
    /// </summary>
    /// <param name="raceAffinity"></param>
    /// <param name="bodyParts">플레이어가 장착한 모든 신체의 배열</param>
    private void UpdatePlayerBodyAffinity(int[] raceAffinity, BodyPart[] bodyParts)
    {
        // TODO : bodyAffinity를 reset하는 메소드를 Player와 관련없도록 분리하자.
        Player.Inst.ResetBodyAffinity();
        for (int bodyIndex = 0; bodyIndex < bodyParts.Length; bodyIndex++)
        {
            BodyPart playerBodyPart = bodyParts[bodyIndex];
            UpdateBodyPartAffinity(raceAffinity, playerBodyPart);
        }
    }

    /// <summary>
    /// playerBodyPart의 종족동화율을 raceAffinity에 Update한다. 단, raceAffinity가
    /// 최소한 한번 초기화된 상태여야 한다.
    /// </summary>
    /// <param name="raceAffinity"></param>
    /// <param name="playerBodyPart"></param>
    private void UpdateBodyPartAffinity(int[] raceAffinity, BodyPart playerBodyPart)
    {
        switch (playerBodyPart.bodyPartType)
        {
            case BodyPartType.Head:
            case BodyPartType.Body:
                raceAffinity[(int)playerBodyPart.race] += 3;
                break;
            case BodyPartType.LeftArm:
            case BodyPartType.RightArm:
            case BodyPartType.LeftLeg:
            case BodyPartType.RightLeg:
                raceAffinity[(int)playerBodyPart.race] += 1;
                break;
            default:
                Debug.Log("wrong item type");
                break;
        }
    }

    /// <summary>
    /// 플레이어의 모든 신체를 참조하여 플레이어의 bodyPartStatus를 업데이트한다.
    /// </summary>
    /// <param name="bodyPartStatus"> = this._bodyPartStatus </param>
    /// <param name="bodyParts"></param>
    private void UpdatePlayerBodyStat(Status bodyPartStatus, BodyPart[] bodyParts)
    {
        bodyPartStatus.ResetStatus();
        for (int bodyIndex =0; bodyIndex < bodyParts.Length; bodyIndex++)
        {
            BodyPart playerBodyPart = bodyParts[bodyIndex];
            UpdateBodyPartStat(bodyPartStatus, playerBodyPart);
        }
    }

    /// <summary>
    /// playerBodyPart의 stat을 bodyPartStatus에 반영한다. 
    /// </summary>
    /// <param name="bodyPartStatus"></param>
    /// <param name="playerBodyPart"></param>
    private void UpdateBodyPartStat(Status bodyPartStatus, BodyPart playerBodyPart)
    {
        bodyPartStatus.atk += playerBodyPart.atk;
        bodyPartStatus.def += playerBodyPart.def;
        bodyPartStatus.dex += playerBodyPart.dex;
        bodyPartStatus.mana += playerBodyPart.mana;
        bodyPartStatus.endurance += playerBodyPart.endurance;
    }
    //private void UpdateBodyStat(BodyPart bodyPart)
    //{
    //    switch (bodyPart.bodyPartType)
    //    {
    //        case BodyPartType.Head:
    //            UpdateBodyPartStat(Player.Inst.head);
    //            break;
    //        case BodyPartType.Body:
    //            UpdateBodyPartStat(Player.Inst.body);
    //            break;
    //        case BodyPartType.LeftArm:
    //            UpdateBodyPartStat(Player.Inst.leftArm);
    //            break;
    //        case BodyPartType.RightArm:
    //            UpdateBodyPartStat(Player.Inst.rightArm);
    //            break;
    //        case BodyPartType.LeftLeg:
    //            UpdateBodyPartStat(Player.Inst.leftLeg);
    //            break;
    //        case BodyPartType.RightLeg:
    //            UpdateBodyPartStat(Player.Inst.rightLeg);
    //            break;
    //        default:
    //            Debug.Log("wrong item Type");
    //            return;
    //    }

    //    _bodyPartStatus.atk -= bodyPart.atk;
    //    _bodyPartStatus.def -= bodyPart.def;
    //    _bodyPartStatus.dex -= bodyPart.dex;
    //    _bodyPartStatus.mana -= bodyPart.mana;
    //    _bodyPartStatus.endurance -= bodyPart.endurance;
    //}

}
