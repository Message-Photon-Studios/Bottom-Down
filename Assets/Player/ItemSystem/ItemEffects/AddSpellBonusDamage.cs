using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpellBonusDamage : ItemEffect
{
    [SerializeField] int damage;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerCombatSystem>().AddBonusDamage(damage);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerCombatSystem>().AddBonusDamage(-damage);
    }

}
