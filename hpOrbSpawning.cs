using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//attached to the hp workbench which spawns hp orb infront of it when you use scraps in it

public class hpOrbSpawning : MonoBehaviour
{
    public TextMeshProUGUI workbenchInterface;
    public Transform player;
    public GameObject hpOrbPrefab;
    private playerStats ps;
    public float interactionDistance = 4f;
    public float makingTime = 10f;
    public int scrapsNeeded = 4;

    private bool makingOrb = false;
    private bool playerEntryTrigger = false;
    //player trigger helps in removing the interface text once after player walk out of range
    //it prevents to clear the text again and again in update() as updating each frame affect the interfact with other workbench

    void Start()
    {
        ps = player.gameObject.GetComponent<playerStats>();
    }
    void Update()
    {
        //Conditions for spawning orbs : player in range, have enough scraps, not already making orbs or having an uncollected orb
        if (Vector3.Distance(player.position, transform.position) <= interactionDistance)
        {
            playerEntryTrigger = true;
            if (makingOrb)
            {
                workbenchInterface.text = " MAKING MEDICINE ";
            }
            if (!ps.hpOrbCollected && !makingOrb)
            {
                workbenchInterface.text = " COLLECT MEDIKIT ";
            }
            if (!makingOrb && ps.hpOrbCollected)
            {
                if (ps.scrapsCollected >= scrapsNeeded)
                {
                    workbenchInterface.text = " E \n MAKE MEDICINE (" + scrapsNeeded + ")";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(MakingHPOrb());
                    }
                }
                else
                {
                    workbenchInterface.text = " NOT ENOUGH SCRAPS (" + scrapsNeeded + ")";
                }
            }


        }
        else if (Vector3.Distance(player.position, transform.position) > interactionDistance && playerEntryTrigger)
        {
            playerEntryTrigger = false;
            workbenchInterface.text = "";
            //This playerEntryTrigger only makes this part run a single time when player go out of workbench range
        }
    }
    private IEnumerator MakingHPOrb()
    {
        makingOrb = true;
        ps.scrapsCollected -= scrapsNeeded;
        ps.scrapsText.text = "" + ps.scrapsCollected;
        yield return new WaitForSeconds(makingTime);
        Instantiate(hpOrbPrefab, gameObject.transform.position + new Vector3(0.2f, 0.5f, 2f), Quaternion.identity);
        ps.hpOrbCollected = false;
        makingOrb = false;
    }
}
