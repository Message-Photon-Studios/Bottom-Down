using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCDMultiplier : ItemEffect
{

    [SerializeField] public float multiplier;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().AddCDMultiplier(multiplier);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().AddCDMultiplier(-multiplier);
    }

}
