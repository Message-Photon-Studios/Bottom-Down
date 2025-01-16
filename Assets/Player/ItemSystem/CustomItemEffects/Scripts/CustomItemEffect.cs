using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomItemEffect : ItemEffect
{
    [SerializeField] List<CustomItem> effects;
    public override void ActivateEffect()
    {
        foreach (CustomItem effect in effects)
        {
            effect.AddEffect();
        }
    }

    public override void DisableEffect()
    {
        foreach (CustomItem effect in effects)
        {
            effect.RemoveEffect();
        }
    }
}

public abstract class CustomItem : ScriptableObject
{

    public abstract void AddEffect();

    public abstract void RemoveEffect();
    public abstract void Effect(PlayerStats player, EnemyStats enemy);
}
