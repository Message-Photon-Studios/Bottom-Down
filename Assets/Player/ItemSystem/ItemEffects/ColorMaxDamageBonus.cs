using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMaxDamageBonus : ItemEffect
{
    [SerializeField] int buff;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().AddColorMaxDamageBuff(buff);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().AddColorMaxDamageBuff(-buff);
    }

}
