using BehaviourTree;
using UnityEngine;

/// <summary>
/// Sets the enemy's velocity directly. Does not change the enemies base speed.
/// </summary>
public class EnemySetVelocity : Node
{
    Rigidbody2D rbody;
    float velocityMag;
    Vector2 velocity;
    bool setDirection = false;

    /// <param name="rbody"></param>
    /// <param name="velocityMag"> doesn't set the enemy's velocity direction but sets the magnitude to exactly this </param>
    public EnemySetVelocity(Rigidbody2D rbody, float velocityMag)
    {
        this.rbody = rbody;
        this.velocityMag = velocityMag;
        setDirection = true;
    }

    public EnemySetVelocity(Rigidbody2D rbody, Vector2 velocity)
    {
        this.rbody = rbody;
        this.velocity = velocity;
        setDirection = true;
    }

    public EnemySetVelocity(Rigidbody2D rbody, Vector2 velocity, float velocityMag)
    {
        this.rbody = rbody;
        this.velocity = velocity.normalized * velocityMag;
        setDirection = true;
    }

    public override NodeState Evaluate()
    {
        if(setDirection)
        {
            rbody.velocity = velocity;
        } else
        {
            rbody.velocity = rbody.velocity.normalized * velocityMag;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
