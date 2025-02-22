using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEditor;

public class QuakeProjectile : Enemy
{
    [SerializeField] int damage;
    [SerializeField] Trigger attackTrigger;
    [SerializeField] float startIdleTime;
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float turnSpeed;
    [SerializeField] Quaternion startRotation;
    [SerializeField] ParticleSystem aim;
    [SerializeField] ParticleSystem rocksFalling;

    protected override Node SetupTree()
    {
        Node root =
        new Selector(new List<Node>{

            new Sequence(new List<Node>{
                new CheckPlayerArea(stats, player, attackTrigger),
                new DamagePlayer(stats, player, damage),
                new SuicideEnemy(stats)
            }),

            new Sequence(new List<Node> {
                new HomTowardsTarget(stats, startRotation, player.transform, 1f, turnSpeed, 2f),
                new CheckBool("startIdle", true),
                new Wait(startIdleTime+Random.Range(0f, 1f)),
                new SetParentVariable("startIdle", false, 2),
                new ParticlesPlay(rocksFalling, false)
            }),

            new Sequence(new List<Node>{
                new CheckBool("startIdle", false),
                new ChangeSpeed(stats, maxSpeed, acceleration),
                new Selector(new List<Node>{
                    new CheckSpeed(stats, 3000f, 3000f),
                    new ParticlesPlay(aim, false)
                }),
            })
        });

        root.SetData("startIdle",  true);
        
        if(Random.Range(0,10) == 0) animator.SetTrigger("crystopher");

        body?.AddTorque(Random.Range(-20f,20f));

        triggersToFlip.Add(attackTrigger);

        return root;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        attackTrigger.DrawTrigger(stats.GetPosition());
    }
    #endif
}
