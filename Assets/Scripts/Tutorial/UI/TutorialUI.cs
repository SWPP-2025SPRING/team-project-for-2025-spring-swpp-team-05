using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image overlay;
    [SerializeField] private TextMeshProUGUI tutorialText;

    public void ShowTutorial(string message)
    {
        if (overlay != null)
        {
            overlay.gameObject.SetActive(true);
            Color color = overlay.color;
            color.a = 0.7f;
            overlay.color = color;
        }

        if (tutorialText != null)
        {
            tutorialText.text = message;
            tutorialText.gameObject.SetActive(true);
        }
    }

    public void HideTutorial()
    {
        overlay?.gameObject.SetActive(false);
        tutorialText?.gameObject.SetActive(false);
    }
}
