using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

/// <summary>
/// Only checks the edge of the platform on one side
/// </summary>
public class CheckPlatformEdgePartly : Node
{
    EnemyStats stats;
    float legPos;
    float checkDownDistance  = 1.5f;
    /// <summary>
    /// Only checks the edge of the platform on one side, decided by if the legPos is larger or smaller than zero.
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="legPos"></param>
    public CheckPlatformEdgePartly(EnemyStats stats, float legPos)
    {
        this.stats = stats;
        this.legPos = legPos;
        this.checkDownDistance = 1.5f;
    }

    public CheckPlatformEdgePartly(EnemyStats stats, float legPos, float checkDownDistance)
    {
        this.stats = stats;
        this.legPos = legPos;
        this.checkDownDistance = checkDownDistance;
    }

    public override NodeState Evaluate()
    {
        bool test = !Physics2D.Raycast(stats.GetPosition() + Vector2.right * legPos*stats.lookDir, Vector2.down , checkDownDistance, GameManager.instance.maskLibrary.onlyGround) ||
                    Physics2D.Raycast(stats.GetPosition() + Vector2.right * legPos*stats.lookDir, Vector2.right *stats.lookDir, .2f, GameManager.instance.maskLibrary.onlyGround);
        state = (test)?NodeState.SUCCESS:NodeState.FAILURE;
        return state;
    }
}
