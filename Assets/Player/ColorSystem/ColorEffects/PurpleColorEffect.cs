using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The purple color effect
/// </summary>
[CreateAssetMenu( menuName = "Gameplay Color/Color Effect/PurpleColorEffect")]
public class PurpleColorEffect : ColorEffect
{
    [SerializeField] float sleepTime;
    [SerializeField] float sleepDamageBonus;
    public override void Apply(GameObject enemyObj, Vector2 impactPoint, GameObject playerObj, float power, bool forcePerspectivePlayer, int extraDamage)
    {
        EnemyStats enemy = enemyObj.GetComponent<EnemyStats>();
        if(enemy != null) 
        {
            if(extraDamage > 0) enemy.DamageEnemy(extraDamage);
            enemy.SleepEnemy(sleepTime * EffectFunction(power), sleepDamageBonus * power, particles);
        }
    }
}
