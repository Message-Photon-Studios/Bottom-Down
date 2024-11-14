using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBuff : ItemEffect
{
    [SerializeField] float maxBuff;
    [SerializeField] float minBuff;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().SetRandomBuff(minBuff, maxBuff);
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().SetRandomBuff(0,0);
    }

}
