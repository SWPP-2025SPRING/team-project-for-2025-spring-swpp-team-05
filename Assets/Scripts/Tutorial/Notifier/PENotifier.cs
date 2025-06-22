using UnityEngine;

public class PENotifier : MonoBehaviour
{
    [SerializeField] private PETask peTask; 

    private bool counted = false;

    private void Start()
    {
        if (peTask == null)
            peTask = FindObjectOfType<PETask>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (counted) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            counted = true;
            peTask?.NotifyProjectileHit();
        }
    }
}
