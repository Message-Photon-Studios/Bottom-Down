using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMaxBonus : ItemEffect
{
    [SerializeField] float buff;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().AddColorMaxBuff(buff);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().AddColorMaxBuff(-buff);
    }

}
