using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFollowPlayerScript : MonoBehaviour
{
    [SerializeField] bool offsetSpell;
    GameObject player;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (offsetSpell)
        {
            offset = transform.position - player.transform.position;
        } else
        {
            offset = new Vector3();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
