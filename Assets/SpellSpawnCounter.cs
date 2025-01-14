using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawnCounter : SpellImpact
{
    [SerializeField] ColorSpell spell;
    private ColorInventory inv;

    private void OnEnable()
    {
        inv = FindObjectOfType<ColorInventory>();
        inv.AddSpellSpawned(spell, 1);
    }

    public override void Impact(Collider2D other, Vector2 impactPoint)
    {
        inv.RemoveSpellSpawned(spell, 1);
    }


}
