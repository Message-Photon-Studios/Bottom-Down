using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawnCounter : SpellImpact
{
    [SerializeField] string spawnKey;
    private ColorInventory inv;

    private void OnEnable()
    {
        inv = FindObjectOfType<ColorInventory>();
        inv.AddSpellSpawned(spawnKey, 1);
    }

    public override void Impact(Collider2D other, Vector2 impactPoint)
    {
        inv.RemoveSpellSpawned(spawnKey, 1);
    }


}
