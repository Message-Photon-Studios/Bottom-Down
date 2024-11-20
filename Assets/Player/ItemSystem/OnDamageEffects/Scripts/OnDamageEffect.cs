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
            GetPlayer().GetComponent<PlayerStats>().onPlayerDamaged += effect.Effect;
        }
    }

    public override void DisableEffect()
    {
        foreach (OnDamageDo effect in effects)
        {
            GetPlayer().GetComponent<PlayerStats>().onPlayerDamaged += effect.Effect;
        }
    }
}

public abstract class OnDamageDo : ScriptableObject
{
    public abstract void Effect(PlayerStats player, EnemyStats enemy);
}
