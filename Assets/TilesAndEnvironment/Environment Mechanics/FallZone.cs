using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FallZone : MonoBehaviour
{
    [SerializeField] int damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().DamagePlayer(damage, null);
            other.GetComponent<PlayerMovement>().ReturnToLastGround();
            other.GetComponent<PlayerMovement>().movementRoot.SetRoot("fallZone", .5f);
        }
    }
}
