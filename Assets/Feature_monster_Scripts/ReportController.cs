using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportController : MonoBehaviour
{
    public float speed = 0.5f;
    private GameObject player;
    private Rigidbody reportRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        reportRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 playerPosition = player.transform.position;
        Vector3 chaseDirection = playerPosition - currentPosition;

        reportRigidbody.velocity = (chaseDirection).normalized * speed;

        Quaternion targetRotation = Quaternion.LookRotation(chaseDirection.normalized, Vector3.up);
        transform.rotation = targetRotation;
    }
}
