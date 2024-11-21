using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/On Damage/Wind Bag")]
public class WindBag : OnDamageDo
{
    [SerializeField] float range;
    [SerializeField] float rangePerStack;
    [SerializeField] float force;
    [SerializeField] float forcePerStack;
    public static int stackSize = 0;

    public override void AddEffect()
    {
        if (stackSize == 0) GameObject.Find("Player").GetComponent<PlayerStats>().onPlayerDamaged += Effect;
        stackSize++;
    }

    public override void Effect(PlayerStats player, EnemyStats hit)
    {
        Debug.Log("" + range + " " + stackSize);
        EnemyStats[] enemies = FindObjectsOfType<EnemyStats>();
        float currentRange = range + rangePerStack * stackSize;
        foreach (EnemyStats enemy in enemies)
        {
            float distance = (enemy.transform.position - player.transform.position).sqrMagnitude;
            if (distance < Mathf.Pow(currentRange , 2))
            {
                distance = (currentRange - Mathf.Sqrt(distance))/ currentRange;
                enemy.GetComponent<Rigidbody2D>().AddForce(((enemy.transform.position - player.transform.position).normalized + new Vector3(0,1,0)).normalized * (force + forcePerStack*stackSize)*distance);
            }
        }
    }

    public override void RemoveEffect()
    {
        stackSize--;
        if (stackSize == 0) GameObject.Find("Player").GetComponent<PlayerStats>().onPlayerDamaged += Effect;
    }
}
