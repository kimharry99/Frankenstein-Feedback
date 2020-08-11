using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAssembly : MonoBehaviour
{
    private BodyPart[] _holdingBodyParts = new BodyPart[30];
    // holding된 BodyPart와 chest의 slotNumber간의 index
    private int[] _index = new int[30];

    // 창고의 BodyPart를 로드해서 holdingbodyParts배열에 저장한다. index 또한 업데이트한다.
    public void HoldBodyPartsFromChest()
    {

    }

    private int _selectedSlotOfChest;
    private BodyPart _selectedBodyPart;
    // selectedBodyPart를 업데이트 한다.
    // UI의 OnClick Event에 붙어서 사용된다.
    public void SelectBodyPart(int holdingSlotNumber)
    {

    }

    private StorageManager _sm = StorageManager.Inst;
    [SerializeField]
    private IntVariable energy;
    // 신체 조립을 실행한다.
    // UI의 신체조립 실행 버튼에 붙어서 사용한다. 
    public void AssemleBody()
    {
        // 선택된 신체가 창고에서 이동되어 플레이어에게 장착된다. 
        // Player가 장착하고 있던 신체는 창고로 이동한다.
    }

    private void SpendEnergy(int energyCost)
    {
        // energy UI는 구현하지 않는다.
    }

    [SerializeField]
    private Status _bodyPartStatus;
    // Player의 신체에 해당하는 스텟을 업데이트한다.
    public void UpdateBodyStat()
    {

    }
}
