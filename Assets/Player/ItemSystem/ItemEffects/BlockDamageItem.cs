using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDamageItem : ItemEffect
{
    [Header("Block Damage")]
    [SerializeField] public int addBlockChance;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().chanceToBlock += addBlockChance;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().chanceToBlock -= addBlockChance;
    }

    public override bool CanBeSpawned()
    {
        float availableBlockChance = GetPlayer().GetComponent<PlayerStats>().chanceToBlock;
        List<BlockDamageItem> effects = ItemSpellManager.instance.GetEffectsInLevel<BlockDamageItem>(this);
        foreach(BlockDamageItem effect in effects)
        {
            availableBlockChance += effect.addBlockChance;
        }

        return availableBlockChance + addBlockChance <= 50;
    }
}
