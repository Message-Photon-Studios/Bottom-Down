using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveColorArmour : ItemEffect
{
    [SerializeField] float armour;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().AddAdaptiveArmour(armour);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().AddAdaptiveArmour(-armour);
    }

}
