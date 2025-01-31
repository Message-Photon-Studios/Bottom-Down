using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawnOnEnemy : SpellImpact
{
    [SerializeField] GameObject[] spawnPrefabsOnEnemy;
    [SerializeField] GameObject[] spawnPrefabsElse;


    public override void Impact(Collider2D other, Vector2 impactPoint)
    {
        if (other.CompareTag("Enemy"))
        {
            
            foreach (GameObject spawnPrefab in spawnPrefabsOnEnemy)
            {
                GameObject obj = GameObject.Instantiate(spawnPrefab, transform.position, transform.rotation) as GameObject;
                obj.GetComponent<ColorSpell>().Initi(spell.GetColor(), spell.GetPower(), spell.GetPlayerObj(), spell.lookDir, spell.GetExtraDamage());
                foreach (SpellEnemyInteraction enemyInteraction in obj.GetComponents<SpellEnemyInteraction>())
                {
                    enemyInteraction.SetEnemy(other);
                }
            }
        } else
        {
            foreach (GameObject spawnPrefab in spawnPrefabsElse)
            {
                GameObject obj = GameObject.Instantiate(spawnPrefab, transform.position, transform.rotation) as GameObject;
                obj.GetComponent<ColorSpell>().Initi(spell.GetColor(), spell.GetPower(), spell.GetPlayerObj(), spell.lookDir, spell.GetExtraDamage());
            }
        }
        
    }
}
