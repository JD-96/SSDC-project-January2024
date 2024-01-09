using System.Collections;
using TMPro;
using UnityEngine;

//Just contains some player details and attached to the player

public class playerStats : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI scrapsText;
    public int scrapsCollected = 0;

    public float baseMaxHealth = 100;
    public float baseMaxStamina = 100;
    public float baseSpeed = 100;
    public float baseAttack = 100;
    public float baseDefense = 100;

    // base stat all 100 represents 100%

    public float maxHealth;
    public float maxStamina;
    private float defense;
    private float attack;
    private float speed;

    private float healthMultiplyer = 1;
    private float staminaMultiplier = 1;
    private float attackMultiplier = 1;
    private float defenseMultiplier = 1;
    private float speedMultiplyer = 1;

    public float currentHP;
    public float currentStamina;

    public bool energyOrbCollected = true;
    public bool hpOrbCollected = true;
    void Start()
    { 
        maxHealth = baseMaxHealth * healthMultiplyer;
        maxStamina = baseMaxStamina * staminaMultiplier;
        defense = baseDefense * defenseMultiplier;
        attack = baseAttack * attackMultiplier;
        speed = baseSpeed * speedMultiplyer;

        currentHP = maxHealth;
        currentStamina = maxStamina;

        healthText.text = "Health   : " + currentHP + " / " + maxHealth;
        staminaText.text = "Stamina : " + currentStamina + " / " + maxStamina;
        scrapsText.text = " Scraps : " + scrapsCollected;
    }
}
