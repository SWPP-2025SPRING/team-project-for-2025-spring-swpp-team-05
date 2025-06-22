using UnityEngine;

public class ProfessorController : MonoBehaviour, IMonsterController
{
    [Header("Rotation")]
    public float rotationSpeed = 60f;
    private int rotationDirection;

    [Header("Field of View")]
    public float fieldOfView = 60f;
    public float viewDistance = 30f;
    public float stunTime = 2f;
    public float eyeHeight = 1.5f;
    private FOVMeshRenderer fovMeshRenderer;

    [Header("Professor")]
    private GameObject player;
    private Animator animator;

    public void SetLevel(int level)
    {
        float stunTimeGrowth = 0.1f;
        stunTime += stunTime * stunTimeGrowth * level;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rotationDirection = Random.value > 0.5f ? 1 : -1;

        // 자식에서 FOVMeshRenderer 찾기
        fovMeshRenderer = GetComponentInChildren<FOVMeshRenderer>();
        fovMeshRenderer.UpdateFOV(fieldOfView, viewDistance, eyeHeight);
    }

    void Update()
    {
        RotateProfessor();
        CheckPlayerInSight();
    }

    void RotateProfessor()
    {
        transform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);
    }

    void CheckPlayerInSight()
    {
        if (player == null || PlayerStatus.instance == null) return;

        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 directionToPlayer = player.transform.position - origin;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewDistance) return;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (angleToPlayer < fieldOfView / 2f)
        {
            // 장애물 없으므로 Raycast 생략
            Debug.Log("Professor sees the player (no raycast)!");
            Stun(stunTime);
        }
    }


    void Stun(float duration)
    {
        if (PlayerStatus.instance != null)
            PlayerStatus.instance.StunPlayer(duration);
    }

    private System.Collections.IEnumerator ResetAttackedFlag()
    {
        yield return new WaitForSeconds(0.2f);
        if (animator != null)
        {
            animator.SetBool("Attacked", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Attacked", true);
            StartCoroutine(ResetAttackedFlag());
        }
    }

    public void EndMonster()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || player == null) return;

        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 forward = transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfView / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfView / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + leftBoundary.normalized * viewDistance);
        Gizmos.DrawLine(origin, origin + rightBoundary.normalized * viewDistance);

        Vector3 dirToPlayer = (player.transform.position - origin).normalized;
        float angleToPlayer = Vector3.Angle(forward, dirToPlayer);
        float distanceToPlayer = Vector3.Distance(origin, player.transform.position);

        bool playerVisible = (angleToPlayer < fieldOfView / 2f && distanceToPlayer <= viewDistance);

        Gizmos.color = playerVisible ? Color.green : Color.gray;
        Gizmos.DrawLine(origin, player.transform.position);
    }

    // Tutorial task purpose
    public void IsPlayerInView()
    {
        if (player == null || PlayerStatus.instance == null) return;

        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 directionToPlayer = player.transform.position - origin;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewDistance) return;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        return angleToPlayer < fieldOfView / 2f;
    }
}
