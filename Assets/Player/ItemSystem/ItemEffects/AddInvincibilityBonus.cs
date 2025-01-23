using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInvincibilityBonus : ItemEffect
{
    [SerializeField] float bonus;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().AddInvincibilityBonus(bonus);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().AddInvincibilityBonus(-bonus);
    }

}
