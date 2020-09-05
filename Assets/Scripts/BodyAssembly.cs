﻿using System.Collections;
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
            _index[i] = StorageManager.Inst.GetIndexFromTable(Type.BodyPart, i);
            if (_index[i] != -1)
            {
                _holdingBodyParts[i] = (BodyPart)StorageManager.Inst.chest.slotItem[_index[i]];
                Debug.Log(i.ToString() + "th HoldingBodyParts: " + _holdingBodyParts[i].ToString());
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
        //Debug.Log(_selectedSlotOfChest.ToString() + _selectedBodyPart.ToString());

        if (_selectedBodyPart)
            HomeUIManager.Inst.UpdateAssembleEnergy(_selectedBodyPart.energyPotential);
        else
            HomeUIManager.Inst.UpdateAssembleEnergy(0);
    }

    [SerializeField]
    private IntVariable energy = null;
    // 신체 조립을 실행한다.
    // StorageManager의 아이템 Add함수, Delete함수를 사용하여 구현
    public void AssemleBody()
    {
        // 선택된 신체가 창고에서 이동되어 플레이어에게 장착된다. 
        // Player가 장착하고 있던 신체는 창고로 이동한다.
        // 에너지를 소비한다.
        Player.Inst.ExchangePlayerBody(_selectedBodyPart, _selectedSlotOfChest);
        SpendEnergy(_selectedBodyPart.energyPotential);
        HoldBodyPartsFromChest();
        HomeUIManager.Inst.UpdateBodyAssemblyHoldingImages();
        HomeUIManager.Inst.UpdateAssembleEnergy(0);
        GameManager.Inst.OnTurnOver(2);
        GeneralUIManager.Inst.UpdateEnergy();
    }

    private void SpendEnergy(int energyCost)
    {
        // energy UI는 구현하지 않는다.
        energy.value -= energyCost;
    }

    #region legacy
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
    #endregion
}
