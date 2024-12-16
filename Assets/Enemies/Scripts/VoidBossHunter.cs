using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class VoidBossHunter : Enemy
{
    [SerializeField] Transform bossTransform;
    [SerializeField] float maxBossDistance;
    [SerializeField] float huntTimer;
    [SerializeField] Trigger attackTrigger;
    [SerializeField] int swordDamage;
    [SerializeField] float swordForce;
    [SerializeField] float timeBetweenAttack;
    [SerializeField] Quaternion startRotation;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>{

            new Sequence(new List<Node>{
                new CheckBool("attackDone", false),
                new NormalAttack("swordAttack", player, swordDamage, swordForce, 0.5f, attackTrigger, stats),
                new SetParentVariable("attackDone", true, 2),
            }),

            new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new CheckBool("attack", false),
                    new CheckBool("idle", false),
                    new CheckPlayerArea(stats, player, attackTrigger),
                    new AnimationTrigger(animator, "attack"),
                    new SetParentVariable("idle", true, 3)
                }),

                new Sequence(new List<Node>{
                    new CheckBool("idle", false),
                    new CheckBool("hunt", true),
                    new LookAtPlayer(stats, player),
                    new HomTowardsTarget(stats, startRotation, player.transform, 1f, 1000, 3f),
                    new AnimationTrigger(animator, "walk"),
                    new Wait(huntTimer),
                    new SetParentVariable("hunt", false, 3)
                }),

                new Sequence(new List<Node>{
                    new CheckBool("idle", false),
                    new CheckBool("hunt", false),
                    new LookAtPlayer(stats, player),
                    new Selector(new List<Node>{
                        new Sequence(new List<Node>{
                            new Inverter(new CheckTargetDistance(stats, "bossTransform", maxBossDistance)),
                            new HomTowardsTarget(stats, startRotation, bossTransform, 1f, 1000, 3f)
                        }),

                        new HomTowardsTarget(stats, startRotation, player.transform, 1f, 1000, 3f),
                    }),

                    new AnimationTrigger(animator, "walk"),
                    new Wait(huntTimer),
                    new SetParentVariable("hunt", true, 3)
                }),


            }),

            new Sequence(new List<Node>{
                new Wait(timeBetweenAttack),
                new SetParentVariable("idle", false, 2)
            }),
        });

        root.SetData("hunt", false);
        root.SetData("bossTransform", bossTransform.gameObject);
        root.SetData("idle", false);
        root.SetData("attack", false);
        root.SetData("attackDone", false);
        root.SetData("swordAttack", false);
        root.SetData("castSpell", false);
        root.SetData("spellMode", false);
        triggersToFlip.Add(attackTrigger);
        return root;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        attackTrigger.DrawTrigger(stats.GetPosition());
    }
#endif
}
