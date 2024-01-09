using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attached to the energy orb prefab which restores player stamina and destroys itself when player collides with it
public class energyOrbDetails : MonoBehaviour
{
    private playerStats ps;
    public float staminaRecover = 30f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ps = other.GetComponent<playerStats>();
            
            //so that current stamina doesnt go above max stamina
            if (ps.currentStamina + staminaRecover <= ps.maxStamina)
            {
                ps.currentStamina += staminaRecover;
            }
            else
            {
                ps.currentStamina = ps.maxStamina;
            }
            ps.staminaText.text = "Stamina : " + ps.currentStamina + " / " + ps.maxStamina;
            ps.energyOrbCollected = true;
            Destroy(gameObject);
        }

    }
}
