using BehaviourTree;
using UnityEngine;

/// <summary>
/// Checks the current velocity of the enemy
/// </summary>
internal class CheckVelocity : Node
{
    private Rigidbody2D body;
    private float targetVelocity;
    private float margin;

    private float sqrMargin;
    private float sqrTargetVelocity;
    private Vector2 direction;

    /// <summary>
    /// Checks if the enemy's combined velocity is within margins of the target velocity
    /// </summary>
    /// <param name="body"></param>
    /// <param name="targetVelocity"></param>
    /// <param name="margin"></param>
    public CheckVelocity(Rigidbody2D body, float targetVelocity, float margin)
    {
        this.body = body;
        this.targetVelocity = targetVelocity;
        this.margin = margin;
        this.direction = Vector2.zero;
        sqrTargetVelocity = targetVelocity * targetVelocity;
        sqrMargin = margin * margin;
    }

    /// <summary>
    /// Checks if the enemy's velocity in the specified direction is within margins of the target velocity
    /// </summary>
    /// <param name="body"></param>
    /// <param name="targetVelocity"></param>
    /// <param name="margin"></param>
    /// <param name="direction"></param>
    public CheckVelocity(Rigidbody2D body, int targetVelocity, float margin, Vector2 direction)
    {
        this.body = body;
        this.targetVelocity = targetVelocity;
        this.margin = margin;
        this.direction = direction.normalized;
        sqrTargetVelocity = targetVelocity * targetVelocity;
        sqrMargin = margin * margin;
    }

    public override NodeState Evaluate()
    {
        if(direction == Vector2.zero)
        {
            float sqrVelocity = body.velocity.sqrMagnitude;
            if(sqrTargetVelocity - sqrMargin < sqrVelocity && sqrTargetVelocity + sqrMargin > sqrVelocity)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        } else
        {
            Vector2 dirVelocity = body.velocity * direction;
            
            if(targetVelocity*direction.x - margin < dirVelocity.x && targetVelocity*direction.x + margin > dirVelocity.x &&
                targetVelocity*direction.y - margin < dirVelocity.y && targetVelocity*direction.y + margin > dirVelocity.y)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}