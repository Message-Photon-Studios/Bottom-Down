using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class Inspired : MonoBehaviour
{
    [SerializeField] GameObject spellToEnable;
    [SerializeField] ColorSpell unlockSpell;
    [SerializeField] public int petrifiedPigmentCost;
    [SerializeField] Sprite inspiredSprite;
    [SerializeField] LocalizedString inspireText;

    [SerializeField] GameObject ui;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text headerText;
    [SerializeField] Animator animator;

    private ItemInventory inventory;
    private bool triggered;

    private UIController UI;

    public void Start() {
        if(spellToEnable) spellToEnable.SetActive(false);
        if(GameManager.instance.IsSpellSpawnable(unlockSpell))
        {
            if(spellToEnable)
            {
                spellToEnable.SetActive(true);
            }
            
            if(animator) animator.SetBool("inspired", true);
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
        }
        
        triggered = false;
        UI = PlayerLevelMananger.instance.playerUi;
        inventory = PlayerLevelMananger.instance.playerInventory;

        costText.text = "Cost: " + petrifiedPigmentCost;
        descriptionText.text = unlockSpell.description.GetLocalizedString();
        headerText.text = "Unlock " + unlockSpell.name;
        ui.SetActive(false);
    }
    
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !triggered && !GameManager.instance.IsSpellSpawnable(unlockSpell)) {
            ui.SetActive(true);
            inventory.EnableInspired(this);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            inventory.DisableInspired();
            ui.SetActive(false);
        }
    }

    public void TriggerUnlock()
    {
        triggered = true;
        UI.inspired?.Invoke(inspiredSprite, inspireText.GetLocalizedString());
        GameManager.instance.UnlockedSpell(unlockSpell);
        if(spellToEnable) 
        {
            spellToEnable.SetActive(true);
            spellToEnable.GetComponent<SpellPickup>().SetSpell(unlockSpell);
        }
        GameManager.instance.AddInspiration(1);
        ui.SetActive(false);

        if(animator) animator.SetBool("inspired", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
