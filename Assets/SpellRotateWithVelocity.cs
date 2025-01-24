using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRotateWithVelocity : MonoBehaviour
{
    Rigidbody2D body;
    int lookDir = 1;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        if (GetComponent<ColorSpell>().lookDir == -1) GetComponent<ColorSpell>().SetDir(1);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(lookDir);
        Vector2 dir = body.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (lookDir == -1) angle += Mathf.PI/2;
        body.MoveRotation(angle);
    }
}
