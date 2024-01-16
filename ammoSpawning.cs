using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Attached to the Ammo Workbench. Spawns ammo infront of it when you use scraps in it
//unlike hp orb and energy orb, there is no ammoDetail script attached to ammo prefab. Instead it's details given in bullet spawning script

public class ammoSpawning : MonoBehaviour
{
    public Transform player;
    public float accessDistance = 4f;
    public int scrapsNeeded = 3;
    public float makingTime = 10f;
    public TextMeshProUGUI workbenchInterface;
    private bool makingBullet = false;
    public bool bulletCollected = true;
    private bool playerEntryTrigger = false;
    //Same player entry trigger as hpOrb
    public GameObject bullet;
    private playerStats ps;
    void Start()
    {
        ps = player.gameObject.GetComponent<playerStats>();
    }
    void Update()
    {
        //Conditions for making ammo : in range of the workbench, having enough scraps, not already making ammo or not collect ammo from workbench
        if (Vector3.Distance(transform.position, player.position) <= accessDistance)
        {
            playerEntryTrigger = true;
            if (!makingBullet)
            {
                if (ps.scrapsCollected >= scrapsNeeded && bulletCollected)
                {
                    workbenchInterface.text = " E \n Make Bullets (" + scrapsNeeded + ")";
                    if(Input.GetKeyDown(KeyCode.E)) 
                    {
                        StartCoroutine(MakeBullet());
                    }
                }
                if(ps.scrapsCollected< scrapsNeeded && bulletCollected)
                {
                    workbenchInterface.text = " NOT ENOUGH SCRAPS (" + scrapsNeeded + ")";
                }
                if (!bulletCollected)
                {
                    workbenchInterface.text = " COLLECT BULLET ";
                }

            }
            if(makingBullet)
            {
                workbenchInterface.text = " MAKING BULLET ";
            }
        }
        else if (Vector3.Distance(transform.position, player.position)  > accessDistance && playerEntryTrigger) 
        {
            playerEntryTrigger = false;
            workbenchInterface.text = "";
        }
    }
    private IEnumerator MakeBullet()
    {
        makingBullet = true;
        ps.scrapsCollected -= scrapsNeeded;
        ps.scrapsText.text = "" + ps.scrapsCollected;
        yield return new WaitForSeconds(makingTime);
        Instantiate(bullet, gameObject.transform.position + new Vector3(0f, 0.5f, -2f), Quaternion.identity);
        bulletCollected = false;
        makingBullet = false;

    }
}
