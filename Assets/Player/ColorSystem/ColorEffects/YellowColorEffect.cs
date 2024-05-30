using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay Color/Color Effect/YellowColorEffect")]
public class YellowColorEffect : ColorEffect
{
    [SerializeField] float effectRange;
    [SerializeField] float force;
    [SerializeField] int maxBounces;
    [SerializeField] GameObject lightning;
    public override void Apply(GameObject enemyObj, Vector2 impactPoint, GameObject playerObj, float power)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> affected = new List<GameObject>();

        foreach (GameObject obj in objs)
        {
            if(obj == null) continue;
            if(obj.GetComponent<EnemyStats>().GetColor()?.GetColorEffect() == this) continue; 
            if((obj.transform.position - enemyObj.transform.position).sqrMagnitude < Mathf.Pow(effectRange*power,2))
            {
                AffectObject(obj, 0, enemyObj);
            }
        }

        int depth = 1;
        int with = affected.Count;

        for (int i = 0; i < affected.Count; i++)
        {
            if(i == with)
            {
                depth ++;
                with = affected.Count;
            }
            if (depth > maxBounces) return;
            if (effectRange*power <= depth) return;

            foreach (GameObject obj in objs)
            {
                if(obj == null) continue;
                if(affected[i] == null) continue;
                if(obj.GetComponent<EnemyStats>().GetColor()?.GetColorEffect() == this) continue; 
                if((obj.transform.position - affected[i].transform.position).sqrMagnitude < Mathf.Pow(effectRange*power-depth,2))
                {
                    AffectObject(obj, depth, affected[i]);
                }
            }

        }


        void AffectObject(GameObject obj, int depth, GameObject source)
        {
            if(affected.Contains(obj)) return;
            GameObject connector = GameObject.Instantiate(lightning, obj.transform.position, obj.transform.rotation);
            connector.GetComponent<LineRenderer>().SetPosition(1, obj.transform.position);
            connector.GetComponent<LineRenderer>().SetPosition(0, source.transform.position);
            connector.GetComponent<LightningAnimator>().SetSource(obj);
            connector.GetComponent<LightningAnimator>().SetSource(source);

            GameObject instantiatedParticles = GameObject.Instantiate(particles, obj.transform.position, obj.transform.rotation);
            Destroy(instantiatedParticles, instantiatedParticles.GetComponent<ParticleSystem>().main.duration*2);
            instantiatedParticles.GetComponent<ParticleSystem>().Play();
            // Set enemy as parent of the particle system
            instantiatedParticles.transform.parent = enemyObj.transform;
            affected.Add(obj);
            Vector3 forceDir =  (enemyObj.transform.position - obj.transform.position);
            if(forceDir.sqrMagnitude > 1f) forceDir = forceDir.normalized;
            if (!obj.GetComponent<EnemyStats>().IsKnockbackImune())
                obj?.GetComponent<Rigidbody2D>()?.AddForce(forceDir);
            obj.GetComponent<EnemyStats>().DamageEnemy(Mathf.RoundToInt(damage*power-depth*5));
            Destroy(connector, 1);
        }
    }
}
