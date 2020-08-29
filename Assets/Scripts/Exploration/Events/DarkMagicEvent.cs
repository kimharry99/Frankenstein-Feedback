using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DarkMagicEvent : RandomEncounterEvent
{
    [Header("Dark Magic Event Field")]
    public int dMagicOptNum;
    public int dMagicCaseNum;
    public bool isLearnDarkMagic = true;
    public bool isSchoolBasement = false;

    private void LearnDarkMagic(bool isLearnDarkMagic)
    {
        Player.Inst.DarkMagic = isLearnDarkMagic;
    }

    public override void Option0()
    {
        if (dMagicOptNum == 0)
            if (dMagicCaseNum == GetOptionCaseNumber(dMagicOptNum))
                LearnDarkMagic(isLearnDarkMagic);
        if (isSchoolBasement)
            GameManager.Inst.OnTurnOver(1);
        DoOption(0);
    }

    public override void Option1()
    {
        if (dMagicOptNum == 1)
            if (dMagicCaseNum == GetOptionCaseNumber(dMagicOptNum))
                LearnDarkMagic(isLearnDarkMagic);
        if (isSchoolBasement)
            GameManager.Inst.OnTurnOver(1);
        base.Option1();
    }

    public override void Option2()
    {
        if (dMagicOptNum == 2)
            if (dMagicCaseNum == GetOptionCaseNumber(dMagicOptNum))
                LearnDarkMagic(isLearnDarkMagic);
        base.Option2();
    }

    public override void Option3()
    {
        if (dMagicOptNum == 3)
            if (dMagicCaseNum == GetOptionCaseNumber(dMagicOptNum))
                LearnDarkMagic(isLearnDarkMagic);
        base.Option3();
    }
}
