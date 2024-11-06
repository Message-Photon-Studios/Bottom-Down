using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class NpcManager : MonoBehaviour, IDataPersistence
{
    public static NpcManager instance = null;
    [SerializeField] public SerializedDictionary<string, NpcData> npcData = new SerializedDictionary<string, NpcData>();
    [SerializeField] private SerializedDictionary<string, NpcData> npcOriginalData = new SerializedDictionary<string, NpcData>();
    
    public void Awake()
    {
        if(instance == null) instance = this;
        else
        {
            Destroy(gameObject);
        }

        npcOriginalData = npcData;
    }
    public Dialogue GetDialogue(string npcName)
    {
        if(!npcData.ContainsKey(npcName))
        {
            Debug.LogError("No default dialogue for " + npcName);
            return null;
        }

        Dialogue toReturn = npcData[npcName].GetDialogue(SceneManager.GetActiveScene().name);
        return toReturn;
    }

    #region SaveLoad
    void IDataPersistence.LoadData(GameData data)
    {
        if(data.npcData != null && data.npcData.Count > 0)
            npcData = data.npcData;
        else
            npcData = npcOriginalData;
    }

    void IDataPersistence.SaveData(GameData data)
    {
        data.npcData = npcData;
    }
    #endregion
}

#region NpcData
[System.Serializable]
public class NpcData
{
    [SerializeField] Dialogue defaultDialogue;
    [SerializeField] SerializedDictionary<string, Dialogue> regionalDefaultDialogues;
    [SerializeField] SerializedDictionary<string, List<Dialogue>> specificDialogues;
    
    public Dialogue GetDialogue(string levelName)
    {
        if(specificDialogues.ContainsKey(levelName))
        {
            List<Dialogue> dialogues = specificDialogues[levelName];
            Dialogue toReturn = dialogues[0];
            specificDialogues[levelName].RemoveAt(0);
            if(specificDialogues[levelName].Count == 0) specificDialogues.Remove(levelName);
            return toReturn;
        }

        if(regionalDefaultDialogues.ContainsKey(levelName))
        {
            return regionalDefaultDialogues[levelName];
        }

        return defaultDialogue;
    }

    public void AddSpecialDialogue (string level, Dialogue dialogue)
    {
        if(specificDialogues.ContainsKey(level))
            specificDialogues[level].Add(dialogue);
        else
            specificDialogues.Add(level, new List<Dialogue>{dialogue});
    }

    public void ChangeDefaultDialogue(Dialogue dialogue)
    {
        defaultDialogue = dialogue;
    }

    public void ChangeDefaultDialogue(string[] levels, Dialogue dialogue)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if(regionalDefaultDialogues.ContainsKey(levels[i]))
                regionalDefaultDialogues[levels[i]] = dialogue;
            else
                regionalDefaultDialogues.Add(levels[i], dialogue);
        }
    }

    public void ChangeDefaultDialogue(string level, Dialogue dialogue)
    {
        ChangeDefaultDialogue(new string[]{level}, dialogue);
    }
}

#endregion

#region Dialogue

[System.Serializable]
public class Dialogue
{
    [TextArea(5,20)] public string[] texts;
} 

#endregion