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

    public override bool CanBeSpawned()
    {

        float availableChance = GetPlayer().GetComponent<PlayerStats>().chanceToColorNearby;
        List<ColorNearbyUncolored> effects = ItemSpellManager.instance.GetEffectsInLevel<ColorNearbyUncolored>(this);
        foreach (ColorNearbyUncolored item in effects)
        {
            availableChance += item.chanceToColorNearby;
        }

        return availableChance + chanceToColorNearby <= 80;
    }
}
