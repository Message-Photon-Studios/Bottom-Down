using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStoredSpellBonus : ItemEffect
{
    // Start is called before the first frame update
    [SerializeField] int bonusSpells; 
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().AddBonusSpell(bonusSpells);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().AddBonusSpell(-bonusSpells);
    }

}