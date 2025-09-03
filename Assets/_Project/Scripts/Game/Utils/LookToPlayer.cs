using UnityEngine;

public class LookToPlayer : MonoBehaviour
{
private Transform target;
    public bool rotateX = true;
    public bool rotateY = true;
    public bool rotateZ = true;
    public Vector3 rotationOffset;
    public float rotationSpeed = 5f;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation *= Quaternion.Euler(rotationOffset);

        Vector3 euler = targetRotation.eulerAngles;
        Vector3 currentEuler = transform.rotation.eulerAngles;

        if (!rotateX) euler.x = currentEuler.x;
        if (!rotateY) euler.y = currentEuler.y;
        if (!rotateZ) euler.z = currentEuler.z;

        Quaternion finalRotation = Quaternion.Euler(euler);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            finalRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
