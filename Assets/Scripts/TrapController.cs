using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapController : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    public float attackRadius = 10f;
    public float attackDamage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Attack() {
        yield return new WaitForSeconds(1f);
        Vector3 playerPosition = player.transform.position;
        Vector3 trapPosition = transform.position;
        if((playerPosition - trapPosition).sqrMagnitude < attackRadius) {
            playerController.Damage(attackDamage);
        }
        Destroy(gameObject);
    }
}
