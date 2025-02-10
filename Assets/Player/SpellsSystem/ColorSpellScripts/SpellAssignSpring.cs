using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAssignSpring : MonoBehaviour
{
    [SerializeField] SpringJoint2D spring;
    [SerializeField] LineRenderer chain;
    [SerializeField] Texture chainTexture;
    [SerializeField] Vector3 anchorOffset;
    [SerializeField] bool delayBreak;
    [SerializeField] float delay;
    [SerializeField] int breakForce;
    [SerializeField] bool destroyOnBreak;
    [SerializeField] float destroyTime;

    GameObject player;
    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        if (chain)
        {
            chain.material.SetTexture("_MainTex", chainTexture);
        }
        if (delayBreak)
        {
            delay += Time.fixedTime;
        }
        player = GetComponent<ColorSpell>().GetPlayerObj();
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (spring == null)
        {
            if (destroyOnBreak) GetComponent<ColorSpell>().SetNewDestroy(destroyTime);
            chain.enabled = false;
            this.enabled = false;
        }
        if (spring)
        {
            spring.connectedAnchor = player.transform.position;
        }
        if (delayBreak && delay < Time.fixedTime)
        {
            spring.breakForce = breakForce;
            spring.enabled = true;
            delayBreak = false;
        }
        if (chain)
        {
            chain.SetPosition(0, transform.position);
            chain.SetPosition(1, player.transform.position + anchorOffset);
        }    
    }
}
