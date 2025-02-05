using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleElementalMovement : MonoBehaviour
{
    [SerializeField] float slideSpeed;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D body;
    [SerializeField] BoxCollider2D box;

    float boxWith = 0;
    bool hasTriggered = false;

    void Awake()
    {
        boxWith = box.size.x;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {   
            bool playerR = (other.transform.position.x > transform.position.x);
            bool spellLookR = (transform.parent.localScale.x > 0);
            /*
                both true -> true;
                plR true & slR false -> false;
                plR false & slR -> false;
                plR false & slR false -> true;
            */

            spriteRenderer.flipX =  !(playerR != spellLookR);
            body.velocity = (new Vector2(slideSpeed*((playerR)?-1:1), 0));
            hasTriggered = true;
        }   
    }

    void Update()
    {
        animator.SetBool("slide", Mathf.Abs(body.velocity.x) > .5f);

        if(!Physics2D.Raycast(transform.position, Vector2.down, 1f, GameManager.instance.maskLibrary.onlyGround))
        {
            box.size = new Vector2(1, box.size.y);
        } else
        {
            box.size = new Vector2(boxWith, box.size.y);
        }
    }
}
