using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottlesDontMixColor : ItemEffect
{
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().dontMixColor = true;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().dontMixColor = false;
    }

}
