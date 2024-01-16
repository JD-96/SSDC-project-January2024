using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Attached to the stamina workbench, which spawns energy orbs when player uses scraps in it.

public class StaminaOrbSpawning : MonoBehaviour
{
    public TextMeshProUGUI workbenchInterface;
    public Transform player;
    public GameObject staminaOrbPrefab;
    private playerStats ps;
    public float interactionDistance = 4f;
    public float makingTime = 10f;
    public int scrapsNeeded = 2;

    private bool makingOrb = false;
    private bool playerEntryTrigger = false;
    //same functionality as hp orb player trigger

    void Start()
    {
        ps = player.gameObject.GetComponent<playerStats>();
    }
    void Update()
    {
        // same conditions to spawn stamina orb as before

        if (Vector3.Distance(player.position,transform.position) <= interactionDistance)
        {
            playerEntryTrigger = true;
            if(makingOrb)
            {
                workbenchInterface.text = " MAKING ENERGY ";
            }
            if(!ps.energyOrbCollected && !makingOrb)
            {
                workbenchInterface.text = " COLLECT ORB ";
            }
            if(!makingOrb && ps.energyOrbCollected)
            {
                if (ps.scrapsCollected >= scrapsNeeded)
                {
                    workbenchInterface.text = " E \n MAKE ENERGY (" + scrapsNeeded + ")";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(MakingEnergyOrb());
                    }
                }
                else
                {
                    workbenchInterface.text = " NOT ENOUGH SCRAPS (" + scrapsNeeded + ")";
                }
            }


        }
        else if (Vector3.Distance(player.position,transform.position) > interactionDistance && playerEntryTrigger)
        {
            playerEntryTrigger = false;
            workbenchInterface.text = "";
        }
    }
    private IEnumerator MakingEnergyOrb()
    {
        makingOrb = true;
        ps.scrapsCollected -= scrapsNeeded;
        ps.scrapsText.text = "" + ps.scrapsCollected;
        yield return new WaitForSeconds(makingTime);
        Instantiate(staminaOrbPrefab, gameObject.transform.position + new Vector3(0.2f, 0.5f, 2f), Quaternion.identity);
        ps.energyOrbCollected = false;
        makingOrb = false;
    }
}
