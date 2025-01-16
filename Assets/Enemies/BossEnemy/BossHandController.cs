using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandController : MonoBehaviour
{
    private GameColor gameColor;

    [SerializeField] private Material defaultColor;
    [SerializeField] GameObject bombTemp;
    [SerializeField] Vector3 spawnBombOffset;
    [SerializeField] Vector2 spawnBombForce;

    public void ChangeColor(GameColor gameColor)
    {
        this.gameColor = gameColor;
        if(gameColor != null)
            GetComponent<SpriteRenderer>().material = gameColor.colorMat;
        else
            GetComponent<SpriteRenderer>().material = defaultColor;
    }


    public void SpawnBomb()
    {
        GameObject bomb = Instantiate(bombTemp, transform.position + spawnBombOffset, transform.rotation);
        bomb.GetComponent<EnemyStats>().SetColor(gameColor);
        bomb.GetComponent<Rigidbody2D>().AddForce(spawnBombForce);
    }
}
