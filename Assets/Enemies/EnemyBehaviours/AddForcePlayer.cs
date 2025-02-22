using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class AddForcePlayer : Node
{
    EnemyStats stats;
    Rigidbody2D player;
    Vector2 force;

    /// <summary>
    /// Adds a force to the player. The forced will be flipped depending on which side the player is compared
    /// to the player. 
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="player"></param>
    /// <param name="force"></param>
    public AddForcePlayer(EnemyStats stats, PlayerStats player, Vector2 force)
    {
        this.stats = stats;
        if(player != null)
            this.player = player.GetComponent<Rigidbody2D>();
        this.force = force;
    }

    public override NodeState Evaluate()
    {
        if(player == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        int dir = (stats.GetPosition().x > player.transform.position.x)?-1:1;
        player.AddForce(force*(Vector2.right*dir+Vector2.up));
        state = NodeState.SUCCESS;
        return state;
    }
}
