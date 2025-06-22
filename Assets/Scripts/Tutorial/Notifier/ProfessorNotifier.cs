using UnityEngine;

public class ProfessorNotifier : MonoBehaviour
{
    [SerializeField] private ProfessorTask professorTask;

    private bool isActive = false;

    private void Start()
    {
        isActive = true;

        if (professorTask == null)
            professorTask = FindObjectOfType<ProfessorTask>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            professorTask?.NotifyArrivedAtGoal();
            isActive = false;           
        }
    }
}
