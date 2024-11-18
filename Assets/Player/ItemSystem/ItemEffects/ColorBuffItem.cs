using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Power up the players colors
/// </summary>
[System.Serializable]
public class ColorBuffItem : ItemEffect
{
    [Header("Color Buff")]
    [SerializeField] GameColor color;
    [SerializeField] bool allColors;
    [SerializeField] float power;
    public override void ActivateEffect()
    {
        ColorInventory inv = GetPlayer().GetComponent<ColorInventory>();
        if (allColors) inv.AddDefaultBuff(power);
        else inv.AddColorBuff(color, power);
    }

    public override void DisableEffect()
    {
        ColorInventory inv = GetPlayer().GetComponent<ColorInventory>();
        if (allColors) inv.AddDefaultBuff(-power);
        else inv.AddColorBuff(color, -power);
    }
}
