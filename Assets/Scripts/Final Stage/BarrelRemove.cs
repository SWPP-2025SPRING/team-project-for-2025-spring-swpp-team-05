using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRemove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrel"))
        {
            Destroy(other.gameObject); // Destroy the barrel when it enters the trigger
        }
    }
}
