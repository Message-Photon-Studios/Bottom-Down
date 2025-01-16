using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckGrounded : Node
{
    EnemyStats stats;
    float legPos;

    bool singleLeg;
    /// <summary>
    /// Returns success if the enemy is grounded
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="legPos"></param>
    public CheckGrounded (EnemyStats stats, float legPos)
    {
        this.stats = stats;
        this.legPos = legPos;
        this.singleLeg = false;
    }

    /// <summary>
    /// Returns success if the enemy is grounded. If singleLeg is true it will only check one leg and the leg will be flipped with look dir.
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="legPos"></param>
    /// <param name="singleLeg"></param>
    public CheckGrounded (EnemyStats stats, float legPos, bool singleLeg)
    {
        this.stats = stats;
        this.legPos = legPos;
        this.singleLeg = singleLeg;
    }
    public override NodeState Evaluate()
    {   
        bool test = false;

        if(!singleLeg) 
        {
            test =  Physics2D.Raycast(stats.GetPosition() + Vector2.right* legPos, Vector2.down, 1f, GameManager.instance.maskLibrary.onlyGround) ||
                    Physics2D.Raycast(stats.GetPosition() - Vector2.right* legPos, Vector2.down, 1f, GameManager.instance.maskLibrary.onlyGround);
        } else
        {
            test =  Physics2D.Raycast(stats.GetPosition() + Vector2.right* legPos*stats.lookDir, Vector2.down, 1f, GameManager.instance.maskLibrary.onlyGround);
        }
        state = test?NodeState.SUCCESS:NodeState.FAILURE;
        return state;
    }
}
