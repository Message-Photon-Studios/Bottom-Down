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
    public Item[] items = new Item[0];
}
