using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellEnemyInteraction : MonoBehaviour
{
    public abstract void SetEnemy(Collider2D enemyCollider);
}
