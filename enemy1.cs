using System.Collections;
using TMPro;
using UnityEngine;

//Just testing a simple enemy AI that makes the enemy move between 2 or more way points given by the user and also has some health

public class enemy1 : MonoBehaviour
{
    public float enemy1health = 25f;
    public float enemy1speed = 2f;
    public TextMeshProUGUI enemyHealthText;
    public Transform[] waypoints;
    public int waypointIndex;

    private void Start()
    {
        enemyHealthText.text = "" + enemy1health;
    }
    private void Update()
    {
        enemyHealthText.transform.LookAt(Camera.main.transform);
        enemyHealthText.transform.Rotate(0f, 180f, 0f);

        enemy1move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && enemy1health>0)
        {
            //enemy takes damage
            bulletDestroyHit tempDamage = collision.gameObject.GetComponent<bulletDestroyHit>();
            enemy1health -= tempDamage.bulletDamage;
            enemyHealthText.text = "" + enemy1health;
            if (enemy1health < 0)
            {
                Destroy(gameObject);
            }

        }
    }
    private void enemy1move()
    {
        //Enemy moves to one way point
        Vector3 targetWaypoint = waypoints[waypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, enemy1speed * Time.deltaTime);

        //If enemy reaches that waypoint, way point index moves to next one
        if(Vector3.Distance(transform.position, targetWaypoint)<0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }

}
