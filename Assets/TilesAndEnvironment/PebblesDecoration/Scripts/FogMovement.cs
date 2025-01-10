using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FogMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float changeDirSpeed;
    [SerializeField] float maxBoxDistance;
    [SerializeField] float fadeSpeed;
    
    SpriteRenderer spriteRenderer;
    Vector2 startPos;
    Vector2 moveDir = Vector2.zero;
    float startAlpha;
    bool resetting;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        startAlpha = spriteRenderer.color.a;
    }

    void Update()
    {

        if(InBox() && !resetting)
        {
            Vector2 changeDir = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
            moveDir += changeDir/100f*changeDirSpeed;
            moveDir.Normalize();

            transform.Translate(moveDir*speed*Time.deltaTime);
        } else if(!resetting)
        {
            resetting = true;
            MoveToStart();
        } else
        {
            MoveToStart();
        }
    }

    void MoveToStart()
    {
        while (spriteRenderer.color.a > 0 && !((Vector2)transform.position).Equals(startPos))
        {
            Color color = spriteRenderer.color;
            color.a -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = color;

            return;
        }

        transform.position = startPos;

        while (spriteRenderer.color.a < startAlpha)
        {
            Color color = spriteRenderer.color;
            color.a += fadeSpeed * Time.deltaTime;
            if(color.a > startAlpha) color.a = startAlpha;
            spriteRenderer.color = color;
            return;
        }

        resetting = false;
        return;
    }

    bool InBox()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        float dist = maxBoxDistance/2;
        if(xPos < startPos.x-dist || xPos > startPos.x + dist || yPos < startPos.y - dist || yPos > startPos.y + dist)
        {
            return false;
        }

        return true;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        if(startPos == null || startPos.Equals(Vector2.zero))
            Handles.DrawWireCube(transform.position, Vector2.one*maxBoxDistance);
        else
            Handles.DrawWireCube(startPos, Vector2.one*maxBoxDistance);
    }
    #endif
}
