using UnityEngine;

//Atatched to the Scraps prefab which increases scraps collected count when player collides with it

public class scrapsDetails : MonoBehaviour
{
    public float rotationSpeed = 2f;
    void Update()
    {
        //A rotating animation just to look good
        transform.Rotate(0, 1 * rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //destroys the scrap and increase scrap collected when player collide with it
            playerStats ps = other.GetComponent<playerStats>();
            ps.scrapsCollected += 1;
            ps.scrapsText.text = "Scraps : " + ps.scrapsCollected;
            Destroy(gameObject);
        }
    }
}
