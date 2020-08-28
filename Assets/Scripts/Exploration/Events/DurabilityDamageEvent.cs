using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DurabilityDamageEvent : ExplorationEvent
{
    [Header("DurabilityDamageEvent Field")]
    [SerializeField]
    private int _damage;

    public override bool GetOptionEnable(int optionIndex)
    {
        return true;
    }

    public override void Option0()
    {
        GetDamage(_damage);
        FinishEvent();
    }

    public override void Option1()
    {
        FinishEvent();
    }

    public override void Option2()
    {
        FinishEvent();
    }

    public override void Option3()
    {
        FinishEvent();
    }

    private void GetDamage(int damage)
    {
        Player.Inst.Durability -= damage;
        GeneralUIManager.Inst.UpdateTextDurability();
    }    
}
