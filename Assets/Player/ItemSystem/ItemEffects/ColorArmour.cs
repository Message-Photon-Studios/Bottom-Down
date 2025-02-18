using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorArmour : ItemEffect
{
    [Header("Color Armour")]
    [SerializeField] public GameColor color;
    [SerializeField] public bool allColor;
    [SerializeField] public float armour;

    public override void ActivateEffect()
    {
        if (allColor)
            GetPlayer().GetComponent<PlayerStats>().AddDefaultArmour(armour);
        else 
            GetPlayer().GetComponent<PlayerStats>().AddColorArmour(color,armour);
    }

    public override void DisableEffect()
    {
        if (allColor)
            GetPlayer().GetComponent<PlayerStats>().AddDefaultArmour(-armour);
        else
            GetPlayer().GetComponent<PlayerStats>().AddColorArmour(color, -armour);
    }
}
