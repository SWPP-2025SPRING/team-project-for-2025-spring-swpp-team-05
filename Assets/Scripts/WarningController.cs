using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    public GameObject trapPrefab;
    public float activateDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("activateRoutine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator activateRoutine() {
        yield return new WaitForSeconds(activateDelay);
        Instantiate(trapPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
