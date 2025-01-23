using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CascadingDamage : ItemEffect
{
    [Header("Cascading Damage")]
    [SerializeField] int maxCascadeDamageIncrease;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerCombatSystem>().maxCascadeDamage += maxCascadeDamageIncrease;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerCombatSystem>().maxCascadeDamage -= maxCascadeDamageIncrease;
    }
}
