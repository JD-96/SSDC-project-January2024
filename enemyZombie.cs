using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

//Attached to the zombie prefab. It has most of the zombie AI details

public class enemyZombie : MonoBehaviour
{
    private playerStats ps;
    private NavMeshAgent agent;
    private BoxCollider boxCollider;
    private float playerDistance;
    private Animation anm;
    private Vector3 previousPosition;
    private bool deathTrigger = false;
    private int takingDamageCount = 1;
    //made it count instead of bool because if u hit 2 bullets in row, the first countdown makes it true before the second countdown
    //completes , making the zombie run to you instantly after taking 2nd or later shots.
    private bool attacking = false;
    private bool randomMotionTrigger = true;
    private float randomMotionTime = 10f;
    private bool playerDetected = false;

    public TextMeshProUGUI healthText;
    private Transform playerTarget;
    public Transform zombieDomain;
    public Transform scraps;
    public float followDistance = 25f;
    public float damageDistance = 2f;
    public float zombieHealth = 30f;

    public AnimationClip walking;
    public AnimationClip running;
    public AnimationClip idle;
    public AnimationClip attack;
    public AnimationClip takeDamage;
    public AnimationClip death;
    public float deathTime;


    void Start()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        healthText.text = "" + zombieHealth;
        ps = playerTarget.gameObject.GetComponent<playerStats>();
        agent = GetComponent<NavMeshAgent>();
        anm = GetComponent<Animation>();
        boxCollider = GetComponent<BoxCollider>();

        previousPosition = transform.position;
        //This updates in each frame later to check if zombie is moving or not to play its required animation
    }

    void Update()
    {
        healthText.transform.LookAt(Camera.main.transform);
        healthText.transform.Rotate(0f, 180f, 0f);
        if (zombieHealth > 0 && takingDamageCount ==1 && attacking == false)
        {
            playerDistance = Vector3.Distance(transform.position, playerTarget.position);

            //zombie following player
            if (playerDistance <= followDistance)
                playerDetected = true;
            if (playerDetected)
            {
                agent.speed = 3.5f;
                agent.SetDestination(playerTarget.position);
                transform.LookAt(playerTarget.position - new Vector3(0f,1f,0f));
                // make sure zombie dont forget you if u just enter and leave its range
                if (playerDistance > followDistance * 2)
                    playerDetected = false;
            }

            //checking for zombie movement
            if (transform.position == previousPosition)
            {
                anm.Play(idle.name);
            }
            else if (playerDetected) //zombie following player
            {
                anm.Play(running.name);
            }
            else //zombie moving but not following player
            {
                anm.Play(walking.name);
            }
            previousPosition = transform.position;

            //zombie in range of attacking player
            if (playerDistance < damageDistance)
            {
                attacking = true;
                anm.Play(attack.name);
                StartCoroutine(Attacking());
            }

            //zombie random motion code:
            //check player outside range and zombie is not moving
            //random range of 10f radius of circle around zombie
            if (playerDistance > followDistance && randomMotionTrigger)
            {
                agent.speed = 1f;
                randomMotionTrigger = false;
                agent.SetDestination(zombieDomain.position + new Vector3(Random.Range(-10f,10f),0f,Random.Range(-10f,10f)));
                randomMotionTime = Random.Range(8f, 12f);
                StartCoroutine(RandomMotionCountdown());

            }

        }
        //zombie dead
        if (zombieHealth <= 0 && deathTrigger == false)
        {
            StartCoroutine(DeathCooldown());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //checking for zombie getting hit by bullet
        if(collision.gameObject.CompareTag("Bullet") && zombieHealth > 0)
        {
            playerDetected = true; //detect player from 2x longer range if he shoots the zombie
            bulletDestroyHit bulletcomponent = collision.gameObject.GetComponent<bulletDestroyHit>();
            zombieHealth -= bulletcomponent.bulletDamage;
            healthText.text = "" + zombieHealth;

            //if zombie is attacking then it doesnt need to play taking damage animation
            if (attacking == false)
            {
                anm.Play(takeDamage.name);
            }
            //when taking damage, the zombie will stay in its position for some time.
            agent.SetDestination(transform.position);
            StartCoroutine(TakingDamage());

        }
    }
    private IEnumerator TakingDamage()
    {
        takingDamageCount++;
        yield return new WaitForSeconds(0.4f);
        takingDamageCount--;
    }
    private IEnumerator DeathCooldown()
    {
        ps.score += 15;
        ps.scoreText.text = " SCORE : " + ps.score;
        boxCollider.size = Vector3.zero;
        deathTrigger = true;
        healthText.text = "";
        anm.Play(death.name);
        yield return new WaitForSeconds(deathTime);
        Instantiate(scraps.gameObject,gameObject.transform.position,Quaternion.identity);
        //spawns a scrap just before dying
        Destroy(gameObject);
    }
    private IEnumerator Attacking()
    {
        yield return new WaitForSeconds(1f);
        attacking = false;
        if (!deathTrigger && Vector3.Distance(transform.position, playerTarget.position) < 2 * damageDistance)
        {
            ps.currentHP -= 10;
            ps.healthText.text = "" + ps.currentHP;
        }
    }

    private IEnumerator RandomMotionCountdown()
    {
        yield return new WaitForSeconds(randomMotionTime);
        randomMotionTrigger = true;
    }
}
