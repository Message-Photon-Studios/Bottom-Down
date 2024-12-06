using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEditor;

public class BossEnemyMain : Enemy
{
    [SerializeField] float wispTimer;
    [SerializeField] GameObject wispTmp;
    [SerializeField] Vector2 spawnOffset;
    [SerializeField] Vector2 spawnForce;
    [SerializeField] int beamDamage = 0;
    [SerializeField] float laserDecaySpeed = .5f;
    [SerializeField] Trigger beamTrigger;
    [SerializeField] float beamTimer;
    [SerializeField] ParticleSystem attackOrb;
    [SerializeField] ParticleSystem attackBeam;
    [SerializeField] ParticleSystem attackAim;

    [SerializeField] float patrollDistance;
    [SerializeField] float patrollIdleTime;

    protected override Node SetupTree()
    {
        
        Node root = new Sequence(new List<Node>{

            new KeepHeight(stats, transform.position.y, 1f),

            new Selector(new List<Node>{
                
                new Sequence(new List<Node>{
                    new CheckBool("sleeping", false),
                    new CheckBool("attack", false),
                    new CheckPlayerArea(stats, player, beamTrigger),
                    new Wait(beamTimer, 2),
                    new LookAtPlayer(stats, player),
                    new SetParentVariable("attack", true, 3),
                    new ParticlesPlay(attackAim, true),
                    new ParticlesPlay(attackOrb, true),
                    new ParticlesPlay(attackBeam, true),
                    }),

                new Sequence(new List<Node> {
                    new CheckBool("attack", true),
                    new Inverter(new CheckPlayerArea(stats, player, beamTrigger)),
                    new Wait(laserDecaySpeed, 0.1f),
                    new ParticlesPlay(attackOrb, false),
                    new ParticlesPlay(attackBeam, false),
                    new ParticlesPlay(attackAim, false),
                    new SetParentVariable("attack", false, 3)
                }),

                new Sequence(new List<Node>{
                    new Wait(wispTimer),
                    new EnemyObjectSpawner(stats, wispTmp, spawnOffset, spawnForce, true)
                }),

                new AirPatroll(stats, body, animator, patrollDistance, 1, patrollIdleTime, .7f, "attack", "move")
            })
        });
        
        triggersToFlip.Add(beamTrigger);
        root.SetData("activateBeam", false);
        root.SetData("attack", false);
        root.SetData("sleeping", false);
        return root;
    }

    public override void DamagePlayer()
    {
        player.DamagePlayer((int)(beamDamage*stats.GetDamageFactor()), stats);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        beamTrigger.DrawTrigger(stats.GetPosition());
        Handles.color = Color.yellow;
        Handles.DrawLine(stats.GetPosition() + Vector2.left* patrollDistance, stats.GetPosition() + Vector2.right* patrollDistance);
    }
#endif
}
