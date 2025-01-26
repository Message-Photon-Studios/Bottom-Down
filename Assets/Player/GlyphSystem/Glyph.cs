using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Glyph : MonoBehaviour
{
    [SerializeField] Sprite itemSprite;
    [SerializeField] Sprite slotSprite;
    [SerializeField] [TextArea(5, 20)] public string description;
    [SerializeField] GameColor glyphColor;

    [SerializeField] bool allowMix;
    [SerializeField] bool divideAmountIfMix;
    [SerializeField] bool allowAnyColor;
    [SerializeField] int normalEffectMinAmount;
    [SerializeField] int strongEffectMinAmount;

    [SerializeField] GlyphEffect[] GlyphEffects;
    [SerializeField] Animator animator;


    public void OnEmptyBottle(GameColor color, int amount)
    {
        if (amount <= 0)
        {
            Destroy(this);
            return;
        }

        if (color != glyphColor && !allowAnyColor)
        {
            if (!allowMix)
            {
                Destroy(this);
                return;
            }
            if (glyphColor.ContainsRootColor(color))
            {
                if (divideAmountIfMix)
                {
                    amount *= (int)color.rootColors.Length / glyphColor.rootColors.Length;
                }
            } else if (color.ContainsRootColor(glyphColor))
            {
                if (divideAmountIfMix)
                {
                    amount *= (int)glyphColor.rootColors.Length / color.rootColors.Length;
                }
            }
            else
            {
                Destroy(this);
                return;
            }
        }

        if (amount < normalEffectMinAmount)
        {
            foreach (GlyphEffect effect in GlyphEffects)
            {
                effect.WeakEffect();
            }
            
        } else if (amount < strongEffectMinAmount)
        {
            foreach (GlyphEffect effect in GlyphEffects)
            {
                effect.NormalEffect();
            }
            animator.SetBool("normal", true);
        } else
        {
            foreach (GlyphEffect effect in GlyphEffects)
            {
                effect.StrongEffect();
            }
            animator.SetBool("strong", true);
        }
    }

}
