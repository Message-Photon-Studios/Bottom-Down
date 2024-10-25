using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
public class ItemLibrary : MonoBehaviour{

    [SerializeField] SerializedDictionary<ItemRarity, SerializedDictionary<ItemRarity, float>> dropPointRarities;
    private ItemCollection[,] itemMatrix = new ItemCollection[0,0];

    void Awake()
    {
        PopulateMatrix();   
    }
    public void PopulateMatrix()
    {
        itemMatrix = new ItemCollection[4,3];

        Debug.Log("--- Populating Item Matrix ------------------");
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Item[] items = Resources.LoadAll<Item>("Items/"+((ItemCategory)i).ToString()+"/"+((ItemRarity)j).ToString());
                Debug.Log("Items/"+((ItemCategory)i).ToString()+"/"+((ItemRarity)j).ToString() + " ---->  matrix[" + i + ":" + j + "], item count = " + items.Length);
                itemMatrix[i,j] = new ItemCollection(items);
            }
        }
        Debug.Log("--- Item Population Done ----------------");
    }

    public ItemCollection[] GetCategory(ItemCategory category)
    {
        ItemCollection[] ret = new ItemCollection[3];
        for (int i = 0; i < 3; i++)
        {
            ret[i] = itemMatrix[(int)category, i];
        }

        return ret;
    }

    public ItemCollection[] GetRarity (ItemRarity rarity)
    {
        ItemCollection[] ret = new ItemCollection[3];
        for (int i = 0; i < 4; i++)
        {
            ret[i] = itemMatrix[i, (int)rarity];
        }

        return ret;
    }

    public ItemCollection GetCollection(ItemCategory category, ItemRarity rarity)
    {
        return itemMatrix[(int)category, (int)rarity];
    }

    public ItemCollection[,] GetSubMatrix(ItemCategory[] includeCategory, ItemRarity[] includeRarity)
    {
        ItemCollection[,] ret = new ItemCollection[includeCategory.Length,includeRarity.Length];

        for (int i = 0; i < includeCategory.Length; i++)
        {
            for (int j = 0; j < includeRarity.Length; j++)
            {
                ret[i,j] = itemMatrix[(int)includeCategory[i],(int)includeCategory[j]];
            }
        }

        return ret;
    }

    /// <summary>
    /// Get a random item. The item will come from one of the included categories and the rarity will be weighted by the drop point rarity;
    /// </summary>
    /// <param name="droppointRarity"></param>
    /// <param name="includeCategories"></param>
    /// <returns></returns>
    public Item GetRandomItem(ItemRarity dropPointRarity, ItemCategory[] includeCategories)
    {
        if(includeCategories.Length == 0)
        {
            Debug.LogWarning("Included item categories is empty");
            return null;
        }

        //Decide weight depending on droppioint rarity
        float[] weight = new float[3];
        for (int i = 0; i < 3; i++)
        {
            weight[i] = dropPointRarities[dropPointRarity][(ItemRarity)i];
        }

        //Pick a random category depending on which are available
        int categoryIndex = Random.Range(0, includeCategories.Length);
        ItemCollection[] category = GetCategory(includeCategories[categoryIndex]);

        //Pick a random rarity determined by the weight
        float randomRarity = Random.Range(1f, 100f)/100f;
        int picker = -1;
        for (int i = 0; i < 3; i++)
        {
            if(randomRarity <= weight[i])
            {
                picker = i;
                break;
            }

            randomRarity -= weight[i];
        }

        //If the random rarity could not be chosen something has gone incredibly wrong.
        if(picker == -1)
        {
            Debug.LogError("Critical error in selecting random item. Picker out if range");
            return null;
        }

        //Pick a random item from the category and rarity
        Item[] items = category[picker].items;
        if(items.Length == 0)
        {
            Debug.LogWarning("Item collection " + ((ItemCategory)categoryIndex).ToString()+"/"+((ItemRarity)picker).ToString() + " is empty!");
            return null;
        }
        Item ret = items[Random.Range(0,items.Length)];

        return ret;
    }
}

public class ItemCollection
{
    public ItemCollection(Item[] items)
    {
        this.items = items;
    }
    public Item[] items;
}

/// <summary>
/// What category the item is in
/// </summary>
public enum ItemCategory
{
    ColorBoost,
    Damage,
    Survival,
    Utility,
}

/// <summary>
/// How rare the item is
/// </summary>
public enum ItemRarity
{
    Common,
    Rare,
    Mythic
}
