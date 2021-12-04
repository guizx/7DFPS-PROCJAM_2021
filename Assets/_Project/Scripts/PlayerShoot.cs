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
    void FixedUpdate()
    {
        if(Input.GetButton("Fire1") && Time.time >= timeToFire){
            Debug.Log(Time.time);
            timeToFire = Time.time + 1.0f/fireRate;
            Shoot();
        }
    }

    void Shoot(){
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot")){
            anim.SetTrigger("Shoot");
        }
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) destination = hit.point;
        else destination = ray.GetPoint(maxDistance);

        InstantiateProjectile(firePoint);
    }

    void InstantiateProjectile(Transform point){
        muzzle.Play();
        var projectileObj = Instantiate (projectilePrefab, point.position, Quaternion.LookRotation(destination)) as GameObject;
        //Vector3 inheritedVelocity = playerRB.GetPointVelocity(playerRB.gameObject.transform.position);
        projectileObj.GetComponent<Rigidbody>().velocity = (destination - point.position).normalized * projectileSpeed;
        //projectileObj.GetComponent<Rigidbody>().AddForce(inheritedVelocity, ForceMode.Impulse);
    }
}
