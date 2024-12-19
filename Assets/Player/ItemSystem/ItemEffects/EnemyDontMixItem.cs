using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDontMixItem : ItemEffect
{
    [Header("Enemy Don't Mix Color")]
    [SerializeField] public int addedChance;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().chanceThatEnemyDontMix += addedChance;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().chanceThatEnemyDontMix -= addedChance;
    }

    /*public override bool CanBeSpawned()
    {
        int availableChance = GetPlayer().GetComponent<PlayerStats>().chanceThatEnemyDontMix;
        List<EnemyDontMixItem> effects = ItemSpellManager.instance.GetEffectsInLevel<EnemyDontMixItem>(this);
        foreach (EnemyDontMixItem item in effects)
        {
            availableChance += item.addedChance;
        }

        return availableChance + addedChance <= 80;
    }*/
}
