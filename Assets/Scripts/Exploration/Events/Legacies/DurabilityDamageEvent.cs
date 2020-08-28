//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DurabilityDamageEvent : StatConstraintEvent
//{
//    [Header("DurabilityDamageEvent Field")]
//    [SerializeField]
//    private int _damage;

//    public override bool GetOptionEnable(int optionIndex)
//    {
//        return true;
//    }

//    public override void Option0()
//    {
//        switch(GetOptionCaseNumber(0))
//        {
//            case 0:
//                Debug.Log("회피 실패");
//                GetDamage(_damage);
//                optionResultTexts[0] = option0CaseResult[0];
//                break;
//            case 1:
//                Debug.Log("회피 성공");
//                optionResultTexts[0] = option0CaseResult[1];
//                break;
//        }
//        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[0]);
//        FinishEvent(resultEvent[0]);
//    }

//    private void GetDamage(int damage)
//    {
//        Player.Inst.Durability -= damage;
//        GeneralUIManager.Inst.UpdateTextDurability();
//    }

//    public override void Option1()
//    {
//    }

//    public override void Option2()
//    {
//    }

//    public override void Option3()
//    {
//    }
//}
