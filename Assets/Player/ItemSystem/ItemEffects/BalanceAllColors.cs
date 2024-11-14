using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceAllColors : ItemEffect
{
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().balanceColors = true;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().balanceColors = false;
    }

}
