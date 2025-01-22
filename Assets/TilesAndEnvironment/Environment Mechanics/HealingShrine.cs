using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class HealingShrine : MonoBehaviour
{
    [SerializeField] int heal;
    [SerializeField] int baseCost;
    [SerializeField] int increaseCost;
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text cost;
    [SerializeField] TMP_Text description;
    [SerializeField] InputActionReference buyAction;
    [SerializeField] List<string> phrases;
    [SerializeField] string healAmoutText;
    Animator animator;



    Action<InputAction.CallbackContext> buy;

    private ItemInventory inventory;
    private PlayerStats player;
    private bool buyable = false;
    private int count = 0;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemInventory>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        cost.text = "Cost: " + CalculatePrice();
        description.text = phrases[0] + "\n" + healAmoutText;
    }

    private void OnEnable()
    {
        buy = (InputAction.CallbackContext ctx) => {Buy(); };
        buyAction.action.performed += buy;
    }

    private void OnDisable()
    {
        buyAction.action.performed -= buy;
    }

    private void UpdateCost()
    {
        cost.text = "Cost: " + CalculatePrice();
        if (inventory.GetCoins() < CalculatePrice())
        {
            cost.color = Color.red;
            buyable = false;
        }
        else
        {
            cost.color = Color.white;
            buyable = true;
        }
        if (count >= phrases.Count) description.text = phrases[phrases.Count-1] + "\n" + healAmoutText;
        else description.text = phrases[count] + "\n" + healAmoutText;
        if (player.GetHealth() >= player.GetMaxHealth()) buyable = false;
    }

    private int CalculatePrice()
    {
        return Mathf.RoundToInt((baseCost + increaseCost * count) * ItemSpellManager.instance.stageCostMultiplier);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateCost();
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(false);
            buyable = false;
        }
    }

    private void Buy()
    {
        if (!buyable) return;
        if (animator.GetBool("heal")) return;
        inventory.PayCost(CalculatePrice());
        player.HealPlayer(heal);
        count++;
        UpdateCost();
        animator.SetBool("heal", true);
    }

    protected void HealDone()
    {
        animator.SetBool("heal", false);
    }
}
