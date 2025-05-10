using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousProfessorController : MonoBehaviour
{
    private GameObject player;
    public float speed = 40f;
    public float attackRange = 100f;
    public float attackDelay = 2f;
    private bool isAttack;
    private Vector3 playerPastPosition;
    public float rotationSpeed = 5f;
    
    [SerializeField] private GameObject warningPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 playerCurrentPosition = player.transform.position;

        if ((currentPosition - playerCurrentPosition).sqrMagnitude < attackRange && !isAttack) {
            Debug.Log("Attack Start");
            isAttack = true;
            StartCoroutine("attackRoutine");
        }

        Vector3 direction = playerPastPosition - currentPosition;
        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    IEnumerator attackRoutine() {
        playerPastPosition = player.transform.position;
        Instantiate(warningPrefab, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(attackDelay);
        isAttack = false;
    }
}
