using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamageEffect : ItemEffect
{
    [SerializeField] List<OnDamageDo> effects;
    public override void ActivateEffect()
    {
        foreach (OnDamageDo effect in effects)
        {
            effect.AddEffect();
        }
    }

    public override void DisableEffect()
    {
        foreach (OnDamageDo effect in effects)
        {
            effect.RemoveEffect();
        }
    }
}

public abstract class OnDamageDo : ScriptableObject
{

    public abstract void AddEffect();

    public abstract void RemoveEffect();
    public abstract void Effect(PlayerStats player, EnemyStats enemy);
}
