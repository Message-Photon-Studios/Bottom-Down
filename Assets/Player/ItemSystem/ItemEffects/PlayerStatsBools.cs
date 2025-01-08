using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsBools : ItemEffect
{
    [SerializeField] bool corrosiveColor;
    public override void ActivateEffect()
    {
        PlayerStats stats = GetPlayer().GetComponent<PlayerStats>();
        if (corrosiveColor) stats.corrosiveColor = true;

    }

    public override void DisableEffect()
    {
        PlayerStats stats = GetPlayer().GetComponent<PlayerStats>();
        if (corrosiveColor) stats.corrosiveColor = false;
    }

}
