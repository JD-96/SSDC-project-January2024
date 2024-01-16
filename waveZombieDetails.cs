using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class waveZombieDetails : MonoBehaviour
{
    //Zombie Stats
    public float zombieStrength = 1f;
    private int scoreMultiplyer;
    public float zombieBaseHealth = 60f;
    public float zombieBaseDamage = 10f;
    private float zombieHealth;
    private float zombieDamage;
    public float zombieSpeed = 5f;
    public float zombieRange = 2f;

    //Zombie Drops
    public GameObject scraps;
    public float dropRate = 60f;

    //Zombie UI
    public TextMeshProUGUI healthText;

    //Zombie AI
    private Transform playerTarget;
    private NavMeshAgent agent;
    private playerStats ps;
    private BoxCollider box;
    private int takingDamageCount = 1;

    private bool attacking = false;
    private bool deathTrigger = true;

    private waveSystem ws;
    //Zombie Animations
    private Animation anim;
    public AnimationClip spawningAnimation;
    public AnimationClip runningAnimation;
    public AnimationClip attackingAnimation;
    public AnimationClip gettingHitAnimation;
    public AnimationClip dyingAnimation;

    void Start()
    {
        ws = GameObject.FindWithTag("Wave").GetComponent<waveSystem>();
        scoreMultiplyer = ws.waveNumber;
        if (ws.waveNumber > 1)
        {
            zombieStrength = zombieStrength + ws.waveNumber / 5.0f;
        }

        zombieHealth = zombieBaseHealth * zombieStrength;
        zombieDamage = zombieBaseDamage * zombieStrength;
        playerTarget = GameObject.FindWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        ps = playerTarget.GetComponent<playerStats>();
        anim = GetComponent<Animation>();
        box = GetComponent<BoxCollider>();

        StartCoroutine(ZombieSpawning());
        agent.speed = zombieSpeed;
    }

    void Update()
    {
        healthText.transform.LookAt(Camera.main.transform);
        healthText.transform.Rotate(0f, 180f, 0f);
        if (!deathTrigger && !attacking)
        {
            FollowPlayer();
            AttackCheck();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !deathTrigger)
        {
            zombieHealth -= 10;
            healthText.text = "" + zombieHealth;
            if (!attacking)
            {
                anim.Play(gettingHitAnimation.name);
            }
            agent.SetDestination(transform.position);
            StartCoroutine(GettingHit());
            if (zombieHealth <=0)
            {
                StartCoroutine(ZombieDying());
            }
        }
    }

    private void FollowPlayer()
    {
        if (takingDamageCount == 1)
        {
            anim.Play(runningAnimation.name);
            agent.SetDestination(playerTarget.position);
        }
        
    }
    private void AttackCheck()
    {
        float distance = Vector3.Distance(transform.position, playerTarget.position);
        if(distance <= zombieRange)
        {
            StartCoroutine(AttackingPlayer());
        }
        
    }

    private IEnumerator ZombieSpawning()
    {
        anim.Play(spawningAnimation.name);
        yield return new WaitForSeconds(4f);
        healthText.text = "" + zombieHealth;
        deathTrigger = false;
    }

    private IEnumerator GettingHit()
    {
        takingDamageCount += 1;
        yield return new WaitForSeconds (0.4f);
        takingDamageCount -= 1;
    }
    private IEnumerator AttackingPlayer()
    {
        attacking = true;
        anim.Play(attackingAnimation.name);
        yield return new WaitForSeconds(1f);
        if (!deathTrigger && Vector3.Distance(transform.position, playerTarget.position) < 2.5f*zombieRange)
        { 
            ps.currentHP -= zombieDamage;
            ps.healthText.text = "" + ps.currentHP; 
        }
        attacking = false;
    }
    private IEnumerator ZombieDying()
    {
        ps.score += 10 * scoreMultiplyer;
        ps.scoreText.text = " SCORE : " + ps.score;
        deathTrigger = true;
        box.size = Vector3.zero;
        healthText.text = "";
        anim.Play(dyingAnimation.name);
        yield return new WaitForSeconds(3f);
        float chance = Random.Range(0,100);
        if (chance <= dropRate)
        {
            Instantiate(scraps.gameObject, gameObject.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
