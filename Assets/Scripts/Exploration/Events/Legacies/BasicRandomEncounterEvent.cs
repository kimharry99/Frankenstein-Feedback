//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using UnityEngine;

//[CreateAssetMenu]
//public class BasicRandomEncounterEvent : RandomEncounterEvent
//{
//    public override bool GetOptionEnable(int optionIndex)
//    {
//        return true;
//    }

//    public override void Option0()
//    {
//        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[0]);
//        FinishEvent(resultEvent[0]);
//    }

//    public override void Option1()
//    {
//        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[1]);
//        FinishEvent(resultEvent[1]);
//    }

//    public override void Option2() 
//    {
//    }

//    public override void Option3() { }

//}
