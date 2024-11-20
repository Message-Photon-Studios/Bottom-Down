using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/On Damage/Wind Bag")]
public class WindBag : OnDamageDo
{
    [SerializeField] float range;
    [SerializeField] float force;

    public override void Effect(PlayerStats player, EnemyStats hit)
    {
        Debug.Log("puff");
        EnemyStats[] enemies = FindObjectsOfType<EnemyStats>();
        foreach (EnemyStats enemy in enemies)
        {
            float distance = (enemy.transform.position - player.transform.position).sqrMagnitude;
            if (distance < Mathf.Pow(range, 2))
            {
                distance = Mathf.Sqrt(distance);
                enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - player.transform.position).normalized * force/distance);
            }
        }
    }
}
