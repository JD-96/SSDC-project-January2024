using System.Collections;
using UnityEngine;

//Attached to the bullet prefab, which has the bullet damage and which destroys the bullet when it hit anything

public class bulletDestroyHit : MonoBehaviour
{
    public float bulletDamage = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //many times the bullet spawns inside the player which destroys it while spawning. It prevent that
        }

        else if (collision.gameObject.CompareTag("Baloon"))
        {
            //Baloon objects are objects like crates which destroys in one hit
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
