using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEditor;

public class Lobber : Enemy
{
    [SerializeField] Trigger shootTrigger;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float shootTimer;
    [SerializeField] Vector2 projectileSpawn;
    [SerializeField] float projectileSpawnUpForce;
    [SerializeField] float projectileForceRandomness;

    protected override Node SetupTree()
    {

        Node root = new Selector(new List<Node>{
            new Sequence(new List<Node>{
                new CheckBool("stoneThrowAttack", true),
                new EnemyObjectSpawner(stats, projectilePrefab, projectileSpawn, Vector2.up*projectileSpawnUpForce, false, projectileForceRandomness),
                new SetParentVariable("stoneThrowAttack", false, 2),
            }),

            
            new Sequence(new List<Node>{
                new CheckPlayerArea(stats, player, shootTrigger),
                new Wait(shootTimer),
                new AnimationTrigger(animator, "lobberSlam")
            }),

        });

        root.SetData("stoneThrowAttack", false);
        triggersToFlip.Add(shootTrigger);
        return root;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {;
        shootTrigger.DrawTrigger(stats.GetPosition());
        Handles.color = Color.green;
        Handles.DrawSolidDisc(stats.GetPosition()+projectileSpawn, Vector3.forward, .1f);
    }
#endif
}
