using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/On Damage/Parry Brush")]
public class ParryBrush : OnDamageDo
{
    [SerializeField] string itemName;
    [SerializeField] int damage;
    [SerializeField] int damagePerStack;
    public static int stackSize = 0;

    public override void AddEffect()
    {
        foreach (Item item in GameObject.Find("Player").GetComponent<ItemInventory>().getItems())
        {
            if (item.name.Equals(itemName)) stackSize++;
        }
        if (stackSize == 1) GameObject.Find("Player").GetComponent<PlayerStats>().onPlayerDamaged += Effect;
    }

    public override void Effect(PlayerStats player, EnemyStats hit)
    {
        hit.DamageEnemy(damage + stackSize * damagePerStack);
    }

    public override void RemoveEffect()
    {
        stackSize = 0;
        foreach (Item item in GameObject.Find("Player").GetComponent<ItemInventory>().getItems())
        {
            if (item.name.Equals(itemName)) stackSize++;
        }
        if (stackSize == 0) GameObject.Find("Player").GetComponent<PlayerStats>().onPlayerDamaged -= Effect;
    }
}
