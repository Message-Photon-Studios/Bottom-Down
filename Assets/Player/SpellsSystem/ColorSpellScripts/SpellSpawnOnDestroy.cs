using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawnOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPrefabs;

    private void OnDestroy()
    {
        ColorSpell spell = GetComponent<ColorSpell>();
        foreach (GameObject spawnPrefab in spawnPrefabs)
        {
            GameObject obj = GameObject.Instantiate(spawnPrefab, transform.position, transform.rotation) as GameObject;
            Debug.Log(spell.GetColor());
            obj.GetComponent<ColorSpell>().Initi(spell.GetColor(), spell.GetPower(), spell.GetPlayerObj(), spell.lookDir, spell.GetExtraDamage());
        }
    }
}
