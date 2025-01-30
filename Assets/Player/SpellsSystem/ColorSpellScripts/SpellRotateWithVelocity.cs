using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRotateWithVelocity : MonoBehaviour
{
    Rigidbody2D body;
    float lookDir = 1;
    // Start is called before the first frame update
    void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        
        if (GetComponent<ColorSpell>().lookDir == -1)
        {
            //body.MoveRotation(180);
        }
    }
    void Start()
    {
        lookDir = GetComponent<ColorSpell>().lookDir;
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = body.velocity;
        if (lookDir == -1) dir = Vector2.Reflect(body.velocity, body.velocity);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        body.MoveRotation(angle);
    }
}
