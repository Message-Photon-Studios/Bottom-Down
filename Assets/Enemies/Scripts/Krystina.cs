using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEditor;

public class Krystina : Enemy
{
    [SerializeField] float patrollIdleTime;

    protected override Node SetupTree()
    {
        
        Node root = new RandomPatroll(stats, body, animator, 1, patrollIdleTime, .4f, "attack", "walk");
        
        root.SetData("attack", false);
        return root;
    }
}
