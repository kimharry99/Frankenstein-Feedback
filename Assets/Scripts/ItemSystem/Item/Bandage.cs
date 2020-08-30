using UnityEngine;

[CreateAssetMenu]
public class Bandage : Consumable
{
    public override bool IsConsumeEnable()
    {
        if (Player.Inst.Durability >= 100)
            return false;
        else
            return true;
        
    }

    public override void UseItem()
    {
        Player.Inst.Durability += 10;
        GeneralUIManager.Inst.UpdateTextDurability();
    }
}
