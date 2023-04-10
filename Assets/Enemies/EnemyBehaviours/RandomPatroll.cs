using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class RandomPatroll : Node
{
    EnemyStats stats;
    float patrollDistance;
    float patrollSpeedFactor;
    float maxIdleTime;
    float idleTimer;
    float patrollStart;
    Rigidbody2D body;
    Animator animator;
    string walkAnimation;
    string stopBool;

    float patrollPoint;

    /// <summary>
    /// Makes the enemy move to random positions within the patroll distance. When the enemy have moved to a position it will idle for a random time and then move to a new
    /// random position. If the enemy moves to an edge, into a wall or into another enemy the enemy will return to its starting position. 
    /// It will return running while the it patrolls, if the patroll is blocked for whatever reason then it will return faliure.
    /// </summary>
    /// <param name="stats"></param>
    /// <param name="body"></param>
    /// <param name="animator"></param>
    /// <param name="patrollDistance">The maximum distance from its starting position that the enemy can move to.</param>
    /// <param name="patrollSpeedFactor">This factor will change the speed of the patroll. It scales with the enemys movemet speed.</param>
    /// <param name="maxIdleTime">The maximum time that the enemy can idle</param>
    /// <param name="legPos">The distance from the middle to the enemys leg check</param>
    /// <param name="stopBool">If this bool is true the enemy will stop moving.</param>
    /// <param name="walkAnimationBool">This animation bool will be set to true when the enemy is moving</param>
    /// <returns></returns>
    public RandomPatroll(EnemyStats stats, Rigidbody2D body, Animator animator, float patrollDistance, float patrollSpeedFactor, float maxIdleTime, float legPos, string stopBool, string walkAnimationBool) :
        base(new List<Node>{new CheckPlatformEdge(stats, legPos)})
    {
        this.stats = stats;
        this.patrollDistance = patrollDistance;
        this.body = body;
        this.patrollSpeedFactor = patrollSpeedFactor;
        this.animator = animator;
        this.maxIdleTime = maxIdleTime;
        this.stopBool = stopBool;
        this.walkAnimation = walkAnimationBool;
        patrollStart = stats.GetPosition().x;
        patrollPoint = stats.GetPosition().x;
        idleTimer = 0.5f;
    }

    public override NodeState Evaluate()
    {
        var stopped = GetData(stopBool);
        if(stats.IsAsleep() || (stopped != null && (bool)stopped)) return NodeState.FAILURE;

        if(idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            if(idleTimer < 0)
            {
                idleTimer = 0;
                animator.SetBool(walkAnimation, true);
            }
            state = NodeState.RUNNING;
            return state;
        }

        bool atEdge = (children[0].Evaluate() == NodeState.SUCCESS);
        if(Mathf.Pow(stats.GetPosition().x-patrollPoint, 2) < 1f && !atEdge)
        {
            patrollPoint = GetRandomPoint();
            idleTimer = UnityEngine.Random.Range(0, maxIdleTime);
            animator.SetBool(walkAnimation, false);
            state = NodeState.RUNNING;
            return state;
        }
        else if(atEdge)
            patrollPoint = patrollStart;

        body.AddForce(new Vector2(((patrollPoint < stats.GetPosition().x)?-1:1)*stats.GetSpeed()*patrollSpeedFactor, 0)*Time.deltaTime);
        state = NodeState.RUNNING;
        return state;
    }

    private float GetRandomPoint()
    {
        int randomDir = UnityEngine.Random.Range(-1, 2);
        float ret = patrollStart + randomDir * patrollDistance;
        return ret;
    }
}
