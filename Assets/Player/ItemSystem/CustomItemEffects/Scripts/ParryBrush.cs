using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/On Damage/Parry Brush")]
public class ParryBrush : CustomItem
{
    [SerializeField] string itemName;
    [SerializeField] int damage;
    [SerializeField] int damagePerStack;

    public override void AddEffect()
    {
        PlayerStats player = GameObject.Find("Player").GetComponent<PlayerStats>();
        if (player.itemVaribles.ContainsKey(itemName))
        {
            player.itemVaribles[itemName]++;
        } else
        {
            player.itemVaribles.Add(itemName, 1);
            player.onPlayerDamaged += Effect;
        }
    }

    public override void Effect(PlayerStats player, EnemyStats hit)
    {
        if(hit == null) return;
        hit.DamageEnemy(damage + player.itemVaribles[itemName] * damagePerStack);
    }

    public override void RemoveEffect()
    {
        PlayerStats player = GameObject.Find("Player").GetComponent<PlayerStats>();
        player.itemVaribles[itemName]--;
        if (player.itemVaribles[itemName] == 0) GameObject.Find("Player").GetComponent<PlayerStats>().onPlayerDamaged -= Effect;
    }
}
