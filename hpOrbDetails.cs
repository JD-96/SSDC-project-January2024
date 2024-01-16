using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attached to hp orb prefab, which recovers player hp and destroys itself when player collides with it
public class hpOrbDetails : MonoBehaviour
{
    private playerStats ps;
    public float hpRecover = 40f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ps = other.GetComponent<playerStats>();

            //to prevent current hp going above max hp
            if (ps.currentHP + hpRecover <= ps.maxHealth)
            {
                ps.currentHP += hpRecover;
            }
            else
            {
                ps.currentHP = ps.maxHealth;
            }
            ps.healthText.text = "" + ps.currentHP;
            ps.hpOrbCollected = true;
            Destroy(gameObject);
        }

    }
}
