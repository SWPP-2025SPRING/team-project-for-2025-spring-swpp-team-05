using UnityEngine;
using System;

public class ProfessorView : MonoBehaviour
{
    [SerializeField] private ProfessorController professorController;

    public event Action OnPlayerEnterSight;
    public event Action OnPlayerExitSight;

    private bool wasPlayerInSight = false;

    void Start()
    {
        if (professorController == null)
            professorController = GetComponent<ProfessorController>();
    }

    void Update()
    {
        bool isPlayerInSight = professorController.IsPlayerInView();

        if (isPlayerInSight && !wasPlayerInSight)
            OnPlayerEnterSight?.Invoke();
        else if (!isPlayerInSight && wasPlayerInSight)
            OnPlayerExitSight?.Invoke();

        wasPlayerInSight = isPlayerInSight;
    }
}
