using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskProgressUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI taskDescriptionText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image completionCheckmark;

    void Awake()
    {
        completionCheckmark?.gameObject.SetActive(false);
    }

    public void SetTask(string description)
    {
        if (taskDescriptionText != null)
            taskDescriptionText.text = description;
        
        if (progressText != null)
            progressText.text = "";
        
        if (completionCheckmark != null)
            completionCheckmark.gameObject.SetActive(false);
    }

    public void UpdateProgress(string progress)
    {
        if (progressText != null)
            progressText.text = progress;
    }

    public void ShowCompletion()
    {
        completionCheckmark?.gameObject.SetActive(true);
    }
}