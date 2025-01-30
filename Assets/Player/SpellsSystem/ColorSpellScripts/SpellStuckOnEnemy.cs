using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorSpell))] 
public class SpellStuckOnEnemy : SpellEnemyInteraction
{
    private Vector3 offset;
    private EnemyStats enemyTarget = null;

    public override void SetEnemy(Collider2D enemyCollider)
    {
        enemyTarget = enemyCollider.GetComponent<EnemyStats>();
        offset = transform.position - enemyCollider.transform.position;
        GetComponent<ColorSpell>().AddObjectAlreadyHit(enemyCollider);
    }

    private void Update()
    {
        
        if (enemyTarget != null)
        {
            transform.position = enemyTarget.transform.position + offset;
            if (enemyTarget.IsDead())
            {
                Destroy(gameObject);
            }
        }
    }
}
