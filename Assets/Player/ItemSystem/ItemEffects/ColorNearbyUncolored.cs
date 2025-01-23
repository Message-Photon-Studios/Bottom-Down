using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorNearbyUncolored : ItemEffect
{
    [Header("Chance To Color Nearby")]
    [SerializeField] public int chanceToColorNearby;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().chanceToColorNearby += chanceToColorNearby;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().chanceToColorNearby -= chanceToColorNearby;
    }
}
