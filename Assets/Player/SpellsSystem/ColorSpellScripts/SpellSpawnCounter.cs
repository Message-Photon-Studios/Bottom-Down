using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawnCounter : MonoBehaviour
{
    [SerializeField] string spawnKey;
    private ColorInventory inv;

    private void OnEnable()
    {
        inv = FindObjectOfType<ColorInventory>();
        inv.AddSpellSpawned(spawnKey, 1);
    }

    private void OnDisable()
    {
        inv.RemoveSpellSpawned(spawnKey, 1);
    }


}
