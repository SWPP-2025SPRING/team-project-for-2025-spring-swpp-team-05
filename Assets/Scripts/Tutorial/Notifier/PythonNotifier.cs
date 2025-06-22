using UnityEngine;

public class PythonNotifier : MonoBehaviour
{
    [SerializeField] private PythonTask pythonTask; 

    private void Start()
    {
        if (pythonTask== null)
            pythonTask = FindObjectOfType<PythonTask>();
    }

    public void OnDestroy()
    {
        pythonTask?.NotifyDebuffTriggered();
    }
}

