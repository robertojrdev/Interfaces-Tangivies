using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour 
{
    private Rigidbody rb;
    private Vector3 initialPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.useGravity = false;
    }

    public void SetRotationSpeed(Vector3 force)
    {
        rb.AddTorque(force, ForceMode.VelocityChange);
    }

    public void SetPosition(Vector3 position)
    {
        rb.position = position;
    }

    public void SetSpeed(Vector3 speed)
    {
        rb.velocity = speed;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
