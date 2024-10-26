using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorArmour : ItemEffect
{
    [Header("Color Armour")]
    [SerializeField] public GameColor color;
    [SerializeField] public float armour;

    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().AddColorArmour(color,armour);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().AddColorArmour(color,-armour);
    }

    public override bool CanBeSpawned()
    {
        float availableArmour = GetPlayer().GetComponent<PlayerStats>().GetColorArmour(color);
        List<ColorArmour> availableEffects = ItemSpellManager.instance.GetEffectsInLevel<ColorArmour>(this);
        foreach (ColorArmour item in availableEffects)
        {
            if(item.color = color)
                availableArmour += item.armour;
        }

        return availableArmour + armour <= 50;
    }
}
