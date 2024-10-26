using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUseColor : ItemEffect
{
    [Header("Block Use Color")]
    [SerializeField] public int blockDrainColor = 10;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().blockDrainColor += blockDrainColor;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<ColorInventory>().blockDrainColor -= blockDrainColor;
    }

    public override bool CanBeSpawned()
    {
        float availableBlock = 0;
        List<BlockUseColor> availableBlocks = ItemSpellManager.instance.GetEffectsInLevel<BlockUseColor>(this);
        foreach (BlockUseColor block in availableBlocks)
        {
            availableBlock += block.blockDrainColor;
        }

        return GetPlayer().GetComponent<ColorInventory>().blockDrainColor + availableBlock + blockDrainColor <= 50;
    }
}
