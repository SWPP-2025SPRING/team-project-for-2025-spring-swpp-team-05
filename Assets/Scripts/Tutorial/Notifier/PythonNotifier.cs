using UnityEngine;

public class PythonNotifier : MonoBehaviour
{
    [SerializeField] private PythonTask pythonTask; 

    private void Start()
    {
        if (pythonTask== null)
            pythonTask = GetComponentInParent<PythonTask>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pythonTask?.NotifyDebuffTriggered();
        }
    }
}

