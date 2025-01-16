using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentHealthShop : NpcUpgradeShop
{
    [SerializeField] int healthIncrease;
    protected override void Shop()
    {
        PermanentUpgradeManager.instance.upgrades.extraHealth += healthIncrease;
        FindObjectOfType<PlayerStats>().AddMaxHealth(healthIncrease);
    }
}
