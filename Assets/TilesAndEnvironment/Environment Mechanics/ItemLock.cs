using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;
/// <summary>
/// This class checks if the player has a special key item, in that case the lock is opened.
/// </summary>
public abstract class ItemLock : MonoBehaviour
{
    [SerializeField] Item key;
    [SerializeField] bool consumeKeyOnUnlock;
    [SerializeField] GameObject lockedUI;
    [SerializeField] GameObject unlockabelUI;
    [SerializeField] InputActionReference unlockAction;
    [SerializeField] AudioSource audioSource;
    Action<InputAction.CallbackContext> unlock;
    bool unlocked = false;

    ItemInventory itemInventory;

    bool unlockable = false;

    void Start()
    {
        itemInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemInventory>();
    }

    private void OnEnable() {
        unlock = (InputAction.CallbackContext ctx) => {Unlock();};
        unlockAction.action.performed += unlock;

        if(unlocked) SetUnlocked();
    }
    
    private void OnDisable() 
    {
        unlockAction.action.performed -= unlock;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !unlocked)
        {
            if(itemInventory.HasItemWithName(key.name))
            {
                unlockable = true;
                unlockabelUI.SetActive(true);
                lockedUI.SetActive(false);
            } else
            {
                lockedUI.SetActive(true);
                unlockabelUI.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player")) 
        {
            lockedUI.SetActive(false);
            unlockabelUI.SetActive(false);
            unlockable = false;
        }
    }

    private void Unlock()
    {
        if(unlocked)
        {
            SetUnlocked();
            return;
        }

        if(unlockable)
        {
            unlocked = true;
            audioSource.Play();
            OpenLock();
            if(consumeKeyOnUnlock)
            {
                itemInventory.RemoveItemWithName(key.name);
            }
            lockedUI.SetActive(false);
            unlockabelUI.SetActive(false);
        }
    }



    /// <summary>
    /// Sets the lock as already being unlocked.
    /// </summary>
    protected abstract void SetUnlocked();

    /// <summary>
    /// Opens the lock as normal.
    /// </summary>
    protected abstract void OpenLock();
}
