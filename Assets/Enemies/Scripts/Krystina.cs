using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEditor;

public class Krystina : Enemy
{
    [SerializeField] int damage;
    [SerializeField] Trigger attackTrigger;
    [SerializeField] float patrollIdleTime;

    protected override Node SetupTree()
    {
        
        Node root = new Selector(new List<Node>{
            new Sequence(new List<Node>{
                new CheckPlayerArea(stats, player, attackTrigger),
                new AnimationBool(animator, "walk", false),
                new DamagePlayer(stats, player, damage)
            }),

            new RandomPatroll(stats, body, animator, 1, patrollIdleTime, .4f, "attack", "walk"),
        });
        
        
        root.SetData("attack", false);
        triggersToFlip.Add(attackTrigger);
        return root;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        attackTrigger.DrawTrigger(stats.GetPosition());
    }
    #endif
}
