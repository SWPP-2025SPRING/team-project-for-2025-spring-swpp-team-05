using UnityEngine;

public class ReportNotifier : MonoBehaviour
{
    [SerializeField] private ReportTask reportTask; 

    void Start()
    {
        if (reportTask == null)
            reportTask = GetComponentInParent<ReportTask>();
    }

    void OnDestroy()
    {
        reportTask?.NotifyMonsterDestroyed();
    }
}
