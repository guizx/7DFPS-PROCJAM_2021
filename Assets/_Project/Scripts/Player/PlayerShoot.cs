using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Animator anim;
    public Camera cam;
    public Rigidbody playerRB;
    public GameObject projectilePrefab;
    public ParticleSystem muzzle;
    public Transform firePoint;
    public float maxDistance, projectileSpeed, fireRate;
    [SerializeField] float timeToFire;
    Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) /*&& Time.time >= timeToFire*/){
            Debug.Log(Time.time);
            timeToFire = Time.time + 1.0f/fireRate;
            anim.SetBool("Shooting", true);
            Shoot();
        }
        else if(Input.GetMouseButtonUp(0)){
            anim.SetBool("Shooting", false);
        }
    }

    void Shoot(){
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot")){
            anim.SetTrigger("Shoot");
        }
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //RaycastHit hit;
        //if(Physics.Raycast(ray, out hit)) destination = hit.point;
        //else destination = ray.GetPoint(maxDistance);
        //destination = ray.direction;
        InstantiateProjectile(ray.direction);
    }

    void InstantiateProjectile(Vector3 direction){
        muzzle.Play();
        //Vector3 direction = destination - firePoint.transform.position;
        Bullet bullet = Instantiate (projectilePrefab, firePoint.position, Quaternion.LookRotation(direction)).GetComponent<Bullet>();
        bullet.Setup(direction.normalized);
        //Vector3 inheritedVelocity = playerRB.GetPointVelocity(playerRB.gameObject.transform.position);
        //Vector3 inheritedVelocity = playerRB.velocity;
        //projectileObj.GetComponent<Rigidbody>().velocity = inheritedVelocity + (destination - point.position).normalized * projectileSpeed;
        //projectileObj.GetComponent<Rigidbody>().velocity = (destination - point.position).normalized * projectileSpeed;
        //projectileObj.GetComponent<Rigidbody>().AddForce(inheritedVelocity, ForceMode.Impulse);
    }
}
