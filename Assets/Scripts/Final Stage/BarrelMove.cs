using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelMove : MonoBehaviour
{
    public float speed = 5f; // Speed of the barrel
    public float radius = 0.25f; // Radius of the barrel
    public float rotationSpeed = 100f; // Speed of the barrel's rotation

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward; // Set the initial direction of the barrel
        Debug.Log("BarrelMove Start: " + direction);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Barrel"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                other.transform.position += Vector3.up * 0.05f;
                rb.velocity = direction * speed; // Set the velocity of the barrel
                float omega = speed / radius; // Calculate angular velocity
                Vector3 rotAxis = Vector3.Cross(Vector3.up, direction); // Calculate rotation axis
                rb.angularVelocity = omega * rotAxis; // Set the angular velocity of the barrel
            }
        }
    }
}
