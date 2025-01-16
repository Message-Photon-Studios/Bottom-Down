using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTurnDirection : MonoBehaviour
{
    [SerializeField] bool inverse;
    int lookDir = 1;
    int oldDir = 1;
    Rigidbody2D body;
    ColorSpell spell;

    private void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        spell = GetComponent<ColorSpell>();
    }
    // Update is called once per frame
    void Update()
    {
        if (body.velocity.x > 0) lookDir = 1;
        if (body.velocity.x < 0) lookDir = -1;
        if (inverse) lookDir *= -1;
        if (lookDir != oldDir)
        {
            oldDir = lookDir;
            spell.SetDir(oldDir);
        }

    }
}
