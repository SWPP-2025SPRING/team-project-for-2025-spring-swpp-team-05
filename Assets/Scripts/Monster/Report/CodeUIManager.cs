using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CodeUIManager : MonoBehaviour
{

    public static CodeUIManager Instance { get; private set; }
    public TextMeshProUGUI prevCodeText;
    public TextMeshProUGUI currentCodeText;
    public TextMeshProUGUI nextCodeText;
    public TextMeshProUGUI nextNextCodeText;

    private bool isActive = true;

    void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 변경 시 유지하고 싶을 경우
    }

    void Start()
    {
        // 초기 UI 상태 설정
        DeactivateCodeUI();
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void UpdateCodeUI(
        char prevCode,
        char currentCode,
        char nextCode,
        char nextNextCode
    )
    {
        if (!isActive)
        {
            ActivateCodeUI(); // UI가 비활성화 상태라면 활성화
        }
        prevCodeText.text = prevCode.ToString();
        currentCodeText.text = currentCode.ToString();
        nextCodeText.text = nextCode.ToString();
        nextNextCodeText.text = nextNextCode.ToString();
    }

    public void ActivateCodeUI()
    {
        if (isActive)
        {
            return; // 이미 활성화되어 있으면 아무 작업도 하지 않음
        }
        prevCodeText.gameObject.SetActive(true);
        currentCodeText.gameObject.SetActive(true);
        nextCodeText.gameObject.SetActive(true);
        nextNextCodeText.gameObject.SetActive(true);
        isActive = true; // 활성화 상태로 변경
    }

    public void DeactivateCodeUI()
    {
        if (!isActive)
        {
            return; // 이미 비활성화되어 있으면 아무 작업도 하지 않음
        }
        prevCodeText.gameObject.SetActive(false);
        currentCodeText.gameObject.SetActive(false);
        nextCodeText.gameObject.SetActive(false);
        nextNextCodeText.gameObject.SetActive(false);
        isActive = false; // 비활성화 상태로 변경
    }

}