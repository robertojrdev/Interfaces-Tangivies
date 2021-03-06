﻿using UnityEngine;

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
        transform.position = position;
    }

    public void SetSpeed(Vector3 speed)
    {
        rb.velocity = speed;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        Explosion.Explode("on hit asteroid", transform.position);
    }

}
