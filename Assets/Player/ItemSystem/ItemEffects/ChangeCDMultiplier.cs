using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCDMultiplier : ItemEffect
{

    [SerializeField] public float multiplier;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().MultiplyCDMultiplier(multiplier);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().MultiplyCDMultiplier(Mathf.Pow(multiplier, -1));
    }

}
