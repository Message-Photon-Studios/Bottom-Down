using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FloatingCrystal : Enemy
{
    [SerializeField] float turnSpeed;
    [SerializeField] float passiveViewRange;
    [SerializeField] float aggressiveMaxDistance;
    [SerializeField] float attackDistance;
    [SerializeField] Quaternion startRotation;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] float patrolDistance;
    [SerializeField] float patrolIdleTime;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>{

            new Sequence(new List<Node>{
                new CheckBool("aggressive", false),
                new CheckPlayerDistance(stats, player, 0, passiveViewRange),
                new SetParentVariable("aggressive", true, 2)
            }),

            new Sequence(new List<Node>{
                new CheckBool("aggressive", true),
                new Inverter(new CheckPlayerDistance(stats, player, 0, aggressiveMaxDistance)),
                new Wait(3, 1),
                new SetParentVariable("aggressive", false, 2)
            }),

            new Sequence(new List<Node>{
                new CheckBool("aggressive", true),
                new CheckPlayerDistance(stats, player, 0, attackDistance),
                new Wait(.3f),
                new EnemyObjectSpawner(stats, attackPrefab, Vector2.zero, Vector2.right, true, 1)
            }),

            new Sequence(new List<Node>{
                new CheckBool("aggressive", true),
                new CheckPlayerDistance(stats, player, attackDistance, aggressiveMaxDistance),
                new LookAtPlayer(stats, player),
                new HomTowardsTarget(stats, startRotation, player.transform, 1f, turnSpeed, .5f),
            }),

            new Sequence(new List<Node>{
                new CheckBool("aggressive", false),
                new AirPatroll(stats, body, animator, patrolDistance, .2f, patrolIdleTime, .5f, "aggressive", "walk")
            }),
        });

        root.SetData("aggressive", false);
        return root;
    }

    protected override void DamageTaken(float damage, Vector2 atPosition)
    {
        if(root != null)
            root.SetData("aggressive", true);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(stats.GetPosition(), Vector3.forward, attackDistance);
        Handles.color = Color.blue;
        Handles.DrawWireDisc(stats.GetPosition(), Vector3.forward, passiveViewRange);
        Handles.color = Color.green;
        Handles.DrawLine(stats.GetPosition() + Vector2.left * patrolDistance, stats.GetPosition() + Vector2.right * patrolDistance);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(stats.GetPosition(), Vector3.forward, aggressiveMaxDistance);
    }
#endif
}
