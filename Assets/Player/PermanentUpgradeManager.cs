using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentUpgradeManager : MonoBehaviour, IDataPersistence
{
    #region Singelton
    public static PermanentUpgradeManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public PermanentUpgrades upgrades;

    void IDataPersistence.LoadData(GameData data)
    {
        upgrades = data.permanentUpgrades;
    }

    void IDataPersistence.SaveData(GameData data)
    {
        data.permanentUpgrades = upgrades;
    }
}

[System.Serializable]
public class PermanentUpgrades
{
    public int extraHealth = 0;
    [SerializeField] string[] items = new string[0];

    public void SetPermanentItems(List<Item> setItems)
    {
        items = new string[setItems.Count];
        for (int i = 0; i < setItems.Count; i++)
        {
            items[i] = ((int)setItems[i].itemCategory + "/" +  (int)setItems[i].itemRarity + "/" + setItems[i].name);
        }
    }

    public Item[] GetPermanentItems()
    {
        Item[] returnItems = new Item[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            string[] splitItem = items[i].Split('/');
            ItemCategory itemCategory = (ItemCategory)(Int32.Parse(splitItem[0]));
            ItemRarity itemRarity = (ItemRarity)(Int32.Parse(splitItem[1]));
            string itemName = splitItem[2];
            returnItems[i] = GameManager.instance.itemLibrary.GetItem(itemCategory, itemRarity, itemName);
        }

        return returnItems;
    }
}
