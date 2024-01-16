using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class waveSystem : MonoBehaviour
{

    public float falloutTimer = 30f;
    public float falloutIncrement = 10f;
    public float recoverTimer = 180f;
    public float zombieSpawnInterval = 2f;
    private float runningTimer;
    public Transform falloutZone;
    public Transform falloutZone2;
     
    public TextMeshProUGUI falloutText;
    public TextMeshProUGUI falloutIndicator;
    public GameObject waveZombie;

    private bool fallout;
    private bool nextZombieSpawn = true;
    public int waveNumber = 0;
    void Start()
    {
        runningTimer = recoverTimer;
        fallout = false;
        falloutIndicator.text = " FALLOUT IN ";
        falloutIndicator.color = Color.green;
        falloutText.color = falloutIndicator.color;
    }
    void Update()
    {
        if (runningTimer > 0)
        {
            runningTimer -= Time.deltaTime;
            falloutText.text = "" + Mathf.Floor(runningTimer / 60) + " : " + Mathf.Round(runningTimer % 60);
        }
        if (runningTimer <= 0)
        {
            if(fallout)
            {
                fallout = false;
                runningTimer = recoverTimer;
                falloutIndicator.text = " FALLOUT IN ";
                falloutIndicator.color = Color.green;
                falloutText.color = falloutIndicator.color;
            }
            else
            {
                waveNumber += 1;
                fallout = true;
                runningTimer = falloutTimer;
                falloutIndicator.text = " FALLOUT TILL ";
                falloutIndicator.color = Color.red;
                falloutText.color = falloutIndicator.color ;
                falloutTimer += falloutIncrement;
                
            }
        }
        if (fallout && nextZombieSpawn)
        {
            StartCoroutine(SpawnWaveZombies());
        }
    }

    private IEnumerator SpawnWaveZombies()
    {
        nextZombieSpawn = false;
        yield return new WaitForSeconds(zombieSpawnInterval);
        nextZombieSpawn = true;
        Vector3 spawnArea = falloutZone.position + new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f));
        Vector3 spawnArea2 = falloutZone2.position + new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f));
        Instantiate(waveZombie , spawnArea , Quaternion.identity);
        Instantiate(waveZombie, spawnArea2, Quaternion.identity);
    }
}
