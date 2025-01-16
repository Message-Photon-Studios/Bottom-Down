using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplimentaryDamage : ItemEffect
{
    [SerializeField] int damage;
    public override void ActivateEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().complimentaryDamage += damage;
    }

    public override void DisableEffect()
    {
        GetPlayer().GetComponent<PlayerStats>().complimentaryDamage -= damage;
    }

}
