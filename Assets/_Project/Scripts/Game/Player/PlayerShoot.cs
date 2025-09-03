using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Animator anim;
    public Camera cam;
    public Rigidbody playerRB;
    public GameObject projectilePrefab, burstPrefab;
    public ParticleSystem muzzle;
    public Transform firePoint;
    public LayerMask enemyLayers;
    public float maxDistance, projectileSpeed, burstRange, regularRate, burstRate;
    float fireRate, timeToFire;
    Vector3 destination;
    LevelController levelController;
    bool burst;

    public UnityEvent OnPrimaryShoot;
    public UnityEvent OnSecondatyShoot;
    // Start is called before the first frame update
    void Start()
    {
        levelController = GameObject.Find("Level").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (burst) fireRate = burstRate;
        else fireRate = regularRate;

        bool isLeftMouseHeld = Mouse.current.leftButton.isPressed;
        bool isLeftMouseUp = Mouse.current.leftButton.wasPressedThisFrame;
        bool isRightMouseHeld = Mouse.current.rightButton.isPressed;
        bool isRightMouseUp = Mouse.current.rightButton.wasPressedThisFrame;




        if (isLeftMouseHeld && Time.time >= timeToFire && !levelController.pause)
        {
            burst = false;
            Debug.Log(Time.time);
            timeToFire = Time.time + 1.0f / fireRate;
            anim.SetBool("Shooting", true);
            Shoot();
        }
        if (isLeftMouseUp)
        {
            anim.SetBool("Shooting", false);
        }
        if (isRightMouseHeld && Time.time >= timeToFire && !levelController.pause)
        {
            burst = true;
            Debug.Log(Time.time);
            timeToFire = Time.time + 1.0f / fireRate;
            anim.SetBool("Shooting", true);
            BurstShoot();
        }
        if (isRightMouseUp)
        {
            anim.SetBool("Shooting", false);
        }


        if (Gamepad.current != null)
        {
            bool isPrimaryShootHeld = Gamepad.current.rightTrigger.isPressed;
            bool isPrimaryShootUp = Gamepad.current.rightTrigger.wasReleasedThisFrame; 

            bool isSecondaryShootHeld = Gamepad.current.leftTrigger.isPressed;
            bool isSecondatyShootUp = Gamepad.current.leftTrigger.wasReleasedThisFrame; 


            if (isPrimaryShootHeld && Time.time >= timeToFire && !levelController.pause)
            {
                burst = false;
                Debug.Log(Time.time);
                timeToFire = Time.time + 1.0f / fireRate;
                anim.SetBool("Shooting", true);
                Shoot();
            }
            if (isPrimaryShootUp)
            {
                anim.SetBool("Shooting", false);
            }
            if (isSecondaryShootHeld && Time.time >= timeToFire && !levelController.pause)
            {
                burst = true;
                Debug.Log(Time.time);
                timeToFire = Time.time + 1.0f / fireRate;
                anim.SetBool("Shooting", true);
                BurstShoot();
            }
            if (isSecondatyShootUp)
            {
                anim.SetBool("Shooting", false);
            }
        }
    }

    void Shoot()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        {
            anim.SetTrigger("Shoot");
        }
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //RaycastHit hit;
        //if(Physics.Raycast(ray, out hit)) destination = hit.point;
        //else destination = ray.GetPoint(maxDistance);
        //destination = ray.direction;
        InstantiateProjectile(ray.direction);
        OnPrimaryShoot?.Invoke();
    }
    void BurstShoot()
    {
        muzzle.Play();
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        var burst = Instantiate(burstPrefab, firePoint);
        Destroy(burst, 1);
        OnSecondatyShoot?.Invoke();
    }

    void InstantiateProjectile(Vector3 direction)
    {
        muzzle.Play();
        //Vector3 direction = destination - firePoint.transform.position;
        Bullet bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction)).GetComponent<Bullet>();
        bullet.Setup(direction.normalized);
        //Vector3 inheritedVelocity = playerRB.GetPointVelocity(playerRB.gameObject.transform.position);
        //Vector3 inheritedVelocity = playerRB.velocity;
        //projectileObj.GetComponent<Rigidbody>().velocity = inheritedVelocity + (destination - point.position).normalized * projectileSpeed;
        //projectileObj.GetComponent<Rigidbody>().velocity = (destination - point.position).normalized * projectileSpeed;
        //projectileObj.GetComponent<Rigidbody>().AddForce(inheritedVelocity, ForceMode.Impulse);
    }
}
