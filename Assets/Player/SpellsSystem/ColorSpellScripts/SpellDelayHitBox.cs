using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDelayHitBox : MonoBehaviour
{
    [SerializeField] float delay;
    float timer = 0;
    // Start is called before the first frame update
    private void OnEnable()
    {
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < delay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            foreach (Collider2D collider in GetComponents<Collider2D>())
            {
                collider.enabled = true;
            }
        }

    }
}
