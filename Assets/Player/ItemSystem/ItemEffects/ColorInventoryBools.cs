using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInventoryBools : ItemEffect
{
    [SerializeField] bool dontMixColor;
    [SerializeField] bool autoRotate;
    [SerializeField] bool chaosBottle;
    public override void ActivateEffect()
    {
        ColorInventory colorInv = GetPlayer().GetComponent<ColorInventory>();
        if (dontMixColor) colorInv.dontMixColor = true;
        if (autoRotate) colorInv.autoRotate = true;
        if (chaosBottle) colorInv.chaosEnabled = true;

    }

    public override void DisableEffect()
    {
        ColorInventory colorInv = GetPlayer().GetComponent<ColorInventory>();
        if (dontMixColor) colorInv.dontMixColor = false;
        if (autoRotate) colorInv.autoRotate = false;
        if (chaosBottle) colorInv.chaosEnabled = false;
    }

}
