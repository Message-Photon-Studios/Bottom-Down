using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// Handles the item spawn points
/// </summary>
[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    [Header("Item settings")]
    [SerializeField] public SpawnPointChance spawnChance = SpawnPointChance.LowChance;
    [SerializeField] bool needsPayment;
    [SerializeField] ItemRarity dropPointRarity;
    [SerializeField] ItemCategory[] availableCategories;
    [SerializeField] public bool setByhand;
    [SerializeField] Item item;
    [SerializeField] EnemyStats spawnFromEnemy;

    [Header("Functional")]
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text cost;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject collectObj, buyObj;

  
    ItemInventory inventory;

    private Coroutine hoverCoroutine;

    int itemCost;
    
    void Start()
    {
        if(spawnFromEnemy != null)
        {
            spawnFromEnemy.onEnemyDeath += SpawnFromEnemy;
        } 
        else if(setByhand)
        {
            SetItem(item, item.itemCost);
        } else
        {
            Item randomItem = GameManager.instance.itemLibrary.GetRandomItem(dropPointRarity, availableCategories);
            if(randomItem == null) 
            {
                Debug.LogWarning("Item pickup of name " + name + " could not get a random item");
                gameObject.SetActive(false);
            }
            else
                SetItem(randomItem, randomItem.itemCost);
        }
    }

    private void SpawnFromEnemy()
    {
        transform.position = spawnFromEnemy.transform.position;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector2.down,100f, GameManager.instance.maskLibrary.onlyGround | GameManager.instance.maskLibrary.onlyPlatforms);
        transform.position = hit.point + Vector2.up*.6f;
        SetItem(item, item.itemCost);
        gameObject.SetActive(true);
        spawnFromEnemy.onEnemyDeath -= SpawnFromEnemy;
    }

    void OnDisable()
    {
        if(spawnFromEnemy != null) spawnFromEnemy.onEnemyDeath -= SpawnFromEnemy;
    }

    /// <summary>
    /// Sets the item for this spawnpoint
    /// </summary>
    /// <param name="setItem"></param>
    public void SetItem(Item setItem, int itemCost)
    {
        this.item = setItem;
        this.itemCost = itemCost;
                
        descriptionText.text = item.description;
        nameText.text = item.name;
        cost.text = "Cost: " + itemCost;

        spriteRenderer.sprite = item.sprite;

        canvas.SetActive(false);
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemInventory>();
        hoverCoroutine = StartCoroutine(hoverAnimation());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!needsPayment)
            {
                inventory.EnablePickUp(this);
                cost.gameObject.SetActive(false);
                collectObj.SetActive(true);
                buyObj.SetActive(false);
            } else
            {
                if(inventory.GetCoins() < itemCost)
                {
                    cost.color = Color.red;
                } else
                {
                    cost.color = Color.white;
                }

                inventory.EnableBuyItem(this);
                cost.gameObject.SetActive(true);
                buyObj.SetActive(true);
                collectObj.SetActive(false);
            }

            canvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!needsPayment)
                inventory.DisablePickUp(this);
            else
                inventory.DisableBuyItem(this);
            canvas.SetActive(false);
            cost.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Is called when this item is picked up
    /// </summary>
    public void PickedUp()
    {
        inventory.AddItem(item);
        GameObject.Destroy(gameObject);
        StopCoroutine(hoverCoroutine);
    }

    /// <summary>
    /// Returns the spawnpoints item;
    /// </summary>
    /// <returns></returns>
    public Item GetItem()
    {
        return item;
    }

    /// <summary>
    /// Returns the cost of this item
    /// </summary>
    /// <returns></returns>
    public int GetItemCost()
    {
        return itemCost;
    }
    private IEnumerator hoverAnimation()
    {
        while (true)
        {
            spriteRenderer.transform.position = new Vector3(
                spriteRenderer.transform.position.x, 
                spriteRenderer.transform.position.y + Mathf.Sin(Time.time * 2) * 0.003f, 
                spriteRenderer.transform.position.z);
            yield return new WaitForFixedUpdate();
        }
    }
}

public enum SpawnPointChance
{
    Guaranteed,
    HighChance,
    LowChance
}
