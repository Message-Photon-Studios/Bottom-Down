using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowMaxedBuff : ItemEffect
{
    [Header("Rainbow Combo Damage")]
    [SerializeField] float power;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().colorRainbowMaxedPower += power;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().colorRainbowMaxedPower -= power;
    }
}
