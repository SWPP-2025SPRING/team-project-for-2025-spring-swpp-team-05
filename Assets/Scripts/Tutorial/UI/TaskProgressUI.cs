using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskProgressUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI taskDescriptionText;
    [SerializeField] private TextMeshProUGUI progressText;

    void Awake()
    {

    }

    public void SetTask(string description)
    {
        if (taskDescriptionText != null)
            taskDescriptionText.text = description;
        
        if (progressText != null)
            progressText.text = "";
    }

    public void UpdateProgress(string progress)
    {
        if (progressText != null)
            progressText.text = progress;
    }

    public void ShowCompletion()
    {
        progressText.text += "\n<color=green>클리어!</color>";
        taskDescriptionText.text = "";
    }
}