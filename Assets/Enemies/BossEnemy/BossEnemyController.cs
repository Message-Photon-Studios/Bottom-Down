using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class BossEnemyController : MonoBehaviour
{
    [SerializeField] GameColor[] bossColors;
    [SerializeField] float changeColorTime;
    [SerializeField] GameObject deathUnlock;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject[] spawnEnemies;
    [SerializeField] BossHandController[] hands;

    public static Action onBossDefeated;

    EnemyStats stats;
    float bossStartHealth;
    bool secondPhase = false;
    PlayerStats player;
    bool playerDied = false;
    float changeColorTimer;
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        bossStartHealth = stats.GetHealth();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        stats.onEnemyDeath += BossDied;
        player.onPlayerDied += PlayerDied;

        foreach (BossHandController hand in hands)
        {
            hand.ChangeColor(stats.GetColor());
        }

        stats.onColorChanged += SetHandsColor;
    }

    void SetHandsColor(GameColor color)
    {
        foreach (BossHandController hand in hands)
        {
            hand.ChangeColor(stats.GetColor());
        }

    }

    void Update()
    {
        if(!secondPhase)
        {
            if(stats.GetHealth() < bossStartHealth/2)
            {
                secondPhase = true;
            }
        }

        changeColorTimer -= Time.deltaTime;
        if(changeColorTimer <= 0)
        {
            stats.SetColor(bossColors[UnityEngine.Random.Range(0, bossColors.Length)], 4);
            changeColorTimer = changeColorTime;
        }
    }

    void BossDied(EnemyStats deadBoss)
    {

        onBossDefeated?.Invoke();
        deathUnlock.SetActive(true);
    }

    void PlayerDied()
    {
        healthBar.SetActive(false);
    }

    public GameObject WispSetupAndSpawnObj(GameObject wisp)
    {
        int i = UnityEngine.Random.Range(0, spawnEnemies.Length);
        return spawnEnemies[i];
    }
}
