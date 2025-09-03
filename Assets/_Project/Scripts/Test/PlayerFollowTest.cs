using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowTest : MonoBehaviour
{
    public GameObject player;
    Rigidbody myRb;
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var diff = player.transform.position - this.transform.position;
        this.transform.LookAt(player.transform);
        myRb.linearVelocity = Vector3.Lerp(Vector3.zero, diff, diff.magnitude);
    }
}
