using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEditor;
using System.Linq;
using UnityEngine.Events;

public class Wisp : Enemy
{
    GameObject spawnEnemy = null;
    public UnityEvent<GameObject> onObjectSpawned;
    GameObject spawned;
    protected override Node SetupTree()
    {
        if(gameObject != null && FindObjectsOfType<BossEnemyController>().Count() > 0)
            spawnEnemy = FindObjectOfType<BossEnemyController>().WispSetupAndSpawnObj(gameObject);

        Node root = new Selector(new List<Node>{
            new Sequence(new List<Node>{
                new CheckBool("objectSpawned", false),
                new Selector(new List<Node>{
                    new EnemyCollide(GetComponent<ColliderCheck>(), "Player"),
                    new CheckGrounded(stats,0.2f)
                }),
                new EnemyObjectSpawner(stats, spawnEnemy, Vector2.zero, Vector2.zero, true, "spawnedObject"),
                new SetParentVariable("objectSpawned", true, 1),
                new SuicideEnemy(stats),
                new ActivateAction<GameObject>(onObjectSpawned, "spawnedObject"),
            }),

            new AirPatroll(stats, body, animator, 10, 1, .1f, 1, "objectSpawned", "walk")
            
        });

        root.SetData("spawnedObject", null);
        root.SetData("objectSpawned", false);
        
        return root;
    }

    public void SetSpawnObject(GameObject spawnObject){
        spawnEnemy = spawnObject;
    }

    private void OnDestroy() {
        onObjectSpawned.RemoveAllListeners();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
    }
#endif
}
