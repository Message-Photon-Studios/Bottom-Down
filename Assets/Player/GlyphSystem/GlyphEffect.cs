using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlyphEffect : MonoBehaviour
{
    public abstract void WeakEffect(GameColor color, int amount);
    public abstract void NormalEffect(GameColor color, int amount);
    public abstract void StrongEffect(GameColor color, int amount);

}
