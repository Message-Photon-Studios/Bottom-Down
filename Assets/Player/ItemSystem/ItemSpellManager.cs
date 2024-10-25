using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the distribution of items within the game
/// </summary>
public class ItemSpellManager : MonoBehaviour
{
    [SerializeField] int itemPop;
    [SerializeField] float stageCostMultiplier = 1;
    [SerializeField] ColorSpell[] levelSpells;

    public void SpawnItems()
    {
        List<ColorSpell> spawnSetSpell = new List<ColorSpell>();
        List<ColorSpell> spawnableSpells = new List<ColorSpell>();

        foreach (ColorSpell spell in levelSpells)
        {
            if(GameManager.instance != null && GameManager.instance.IsSpellSpawnable(spell))
                spawnableSpells.Add(spell);
            else if(GameManager.instance == null)
            {
                spawnableSpells.Add(spell);
            }
        }

        spawnSetSpell.AddRange(spawnableSpells);
        
        
        List<GameObject> highSpawnChance = new List<GameObject>();
        List<GameObject> lowSpawnChance = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
        {
            ItemPickup pickup = obj.GetComponent<ItemPickup>();
            if(pickup == null) continue;
            if(obj == null) continue;
            if(pickup.spawnChance == SpawnPointChance.Guaranteed) continue;

            if(pickup.spawnChance == SpawnPointChance.HighChance) highSpawnChance.Add(obj);
            else lowSpawnChance.Add(obj);
        }


        //Purge items until they reached the allowed item pop count.
        while(highSpawnChance.Count + lowSpawnChance.Count > itemPop)
        {
            if(lowSpawnChance.Count > 0)
            {
                int i = Random.Range(0, lowSpawnChance.Count);
                lowSpawnChance[i].SetActive(false);
                lowSpawnChance.RemoveAt(i);
            } else
            {
                int i = Random.Range(0, highSpawnChance.Count);
                highSpawnChance[i].SetActive(false);
                highSpawnChance.RemoveAt(i);
            }
        }    

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SpellItem"))
        {
            obj.GetComponent<SpellPickup>().RandomSpawnDestroy();
            if(obj == null) continue;
            int rng = UnityEngine.Random.Range(0,spawnSetSpell.Count);
            ColorSpell spell = spawnSetSpell[rng];
            spawnSetSpell.RemoveAt(rng);
            obj.GetComponent<SpellPickup>().SetSpell(spell);
            if(spawnSetSpell.Count <= 0) spawnSetSpell.AddRange(spawnableSpells);
        }
    }
}
