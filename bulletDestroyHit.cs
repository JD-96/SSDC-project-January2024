using System.Collections;
using TMPro;
using UnityEngine;

//Attached to the bullet prefab, which has the bullet damage and which destroys the bullet when it hit anything

public class bulletDestroyHit : MonoBehaviour
{
    public float bulletDamage = 10f;
    public float chance = 30;
    public GameObject scraps;
    private playerStats ps;
    private TextMeshProUGUI scoreText;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //many times the bullet spawns inside the player which destroys it while spawning. It prevent that
        }

        else if (collision.gameObject.CompareTag("Baloon"))
        {
            ps = GameObject.FindWithTag("Player").GetComponent<playerStats>();
            scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
            //Baloon objects are objects like crates which destroys in one hit
            if (Random.Range(0, 100) <= chance)
            {
                //chance to drop scraps
                Instantiate(scraps,transform.position,Quaternion.identity);
            }
            ps.score += 2;
            scoreText.text = "SCORE : " + ps.score;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
