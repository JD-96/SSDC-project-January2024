using System.Collections;
using TMPro;
using UnityEngine;

//Attached to player, spawns bullet infront of the camera and gives velocity to it when it is spawned

public class bulletSpawning : MonoBehaviour
{
    public ammoSpawning amm;
    public GameObject bullet;
    public Animator gun;
    public float bulletSpeed = 400f;
    public float despawnTime = 5f;
    public float bulletOffsetX = 1f;
    public float bulletCooldown = 0.2f;
    public float reloadCooldown = 3f;
    public int maxAmmo = 30;
    public int currentAmmo = 30;

    public TextMeshProUGUI viewAmmo;

    private bool bulletTrigger = true;
    private bool reloading = false;
    private void Start()
    {
        viewAmmo.text = "AMMO : " + currentAmmo + " / " + maxAmmo;
    }
    void FixedUpdate()
    {
        if (((Input.GetKey(KeyCode.R) && maxAmmo != 0 && currentAmmo != 30) || currentAmmo == 0 && maxAmmo != 0) && reloading == false)
        {
            //Conditions for reloading = press R when u are not reloading or your clip becomes empty when you have some ammo in magazin
            reloading = true;
            gun.Play("Reloading"); //Reloading animation overwrites idle and firing animations
            StartCoroutine(ReloadHandel());    
        }
        else if (Input.GetMouseButton(0) && currentAmmo>=1 && bulletTrigger==true && reloading==false)
        {
            //the trigger make sure that we are not spamclicking fire and also we dont fire when reloading
            gun.Play("GunFiring"); //Firing animation overwrites idle animation
            SpawnBullet();
            currentAmmo -= 1;
        }

    }
    private void SpawnBullet()
    {
        StartCoroutine(BulletCountdown()); //Firing Cooldown. Spawn Bullet() can be called once everytime this cooldown is over
        GameObject shootedBullet = Instantiate(bullet,Camera.main.transform.position + Camera.main.transform.forward * bulletOffsetX ,Quaternion.identity);
        shootedBullet.transform.rotation = Camera.main.transform.rotation;
        Vector3 shootDirection = Camera.main.transform.forward;
        shootedBullet.GetComponent<Rigidbody>().AddForce(shootDirection * bulletSpeed * Time.deltaTime);
        Destroy(shootedBullet, despawnTime);


    }
    private IEnumerator ReloadHandel()
    {
        yield return new WaitForSeconds(reloadCooldown); //reload cooldown
        //Reloading logic
        if (currentAmmo + maxAmmo <= 30)
        {
            currentAmmo = currentAmmo + maxAmmo;
            maxAmmo = 0;
        }
        else
        {
            maxAmmo = currentAmmo + maxAmmo - 30;
            currentAmmo = 30;
        }
        viewAmmo.text = "AMMO : " + currentAmmo + " / " + maxAmmo;
        reloading = false;
    }
    
    private IEnumerator BulletCountdown()
    {
        bulletTrigger = false;
        yield return new WaitForSeconds(bulletCooldown);

        viewAmmo.text = "AMMO : " + currentAmmo + " / " + maxAmmo;
        bulletTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            //When collide with Ammo, give 30 clip ammo
            maxAmmo+=30;
            viewAmmo.text = "AMMO : " + currentAmmo + " / " + maxAmmo;
            amm.bulletCollected = true; //Triggers ammo collected from ammo workbench
            Destroy(other.gameObject);
        }
    }
}

