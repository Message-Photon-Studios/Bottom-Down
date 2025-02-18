using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellImactOnVelocity : SpellImpact
{
    [SerializeField] float velocityBonusPower;
    [SerializeField] float minVelocity, maxVelocity;

    [SerializeField] protected bool forcePerspectivePlayer;
    [SerializeField] protected bool triggerImpactSpells;
    [SerializeField] public ParticleSystem onImpactParticles;

    static public UnityAction onSpellImpact;

    Rigidbody2D body;
    float power;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        power = spell.GetPower();
    }

    public override void Impact(Collider2D other, Vector2 impactPoint)
    {
        if (other.CompareTag("Enemy"))
        {
            float collisionCheckDeadZone = .75f;
            if (Vector2.Distance(other.transform.position, transform.position) > collisionCheckDeadZone)
            {
                Vector2 spellPos = transform.position;
                Vector2 enemyPos = other.transform.position;
                Vector2 rayCastOrigin = spellPos + ((enemyPos - spellPos).normalized * collisionCheckDeadZone);
                Vector2 rayCastDirection = enemyPos - rayCastOrigin;
                float rayCastDistance = Vector2.Distance(enemyPos, rayCastOrigin);

                RaycastHit2D test = Physics2D.Raycast(rayCastOrigin, rayCastDirection, rayCastDistance, GameManager.instance.maskLibrary.onlyGround);
                if (test.collider != null)
                {
                    return;
                }
            }

            EnemyStats enemy = other.gameObject.GetComponent<EnemyStats>();

            if (enemy != null)
            {
                float powerbonus = ((body.velocity.sqrMagnitude - minVelocity) / maxVelocity) * velocityBonusPower;
                if (powerbonus <= 0) powerbonus = 0;
                else if (powerbonus >= velocityBonusPower) powerbonus = velocityBonusPower;

                Debug.Log(power + powerbonus);

                spell.GetColor().ApplyColorEffect(other.gameObject, transform.position, spell.GetPlayerObj(), spell.GetPower() + powerbonus, forcePerspectivePlayer, spell.GetExtraDamage());
                if (triggerImpactSpells && !spell.castOnSpellImpact) onSpellImpact?.Invoke();
                enemy.enemySounds?.PlaySpellHit();
            }
        }

        var instantiatedParticles = GameObject.Instantiate(onImpactParticles, impactPoint, transform.rotation);
        // Change the particle color to the color of the spell
        var main = instantiatedParticles.main;
        main.startColor = spell.GetColor().plainColor;
        instantiatedParticles.Play();
        Destroy(instantiatedParticles.gameObject, instantiatedParticles.main.duration * 2);
    }
}
