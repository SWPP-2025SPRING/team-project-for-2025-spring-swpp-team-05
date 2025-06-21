using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using TMPro;
using UnityEngine.UI;

public enum DebufType
{
    None,
    Stun,
    ControlInversion,
    Slip,
    Mosquito,
    CameraTilt,
    CameraBlur,
    CameraFlip
}

public class DebufManager : MonoBehaviour
{
    public static DebufManager Instance { get; private set; }

    public TextMeshProUGUI debufText;
    private CancellationTokenSource cts = new CancellationTokenSource();

    private void Awake()
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
        debufText.text = ""; // 초기 상태로 텍스트 비우기
    }

    public void UpdateDebufText(DebufType debufType = DebufType.None)
    {
        switch (debufType)
        {
            case DebufType.Stun:
                StartCoroutine(PulseText(debufText.transform, 1f, 1.2f, 2f, cts.Token));
                debufText.text = "Stunned!";
                break;
            case DebufType.ControlInversion:
                StartCoroutine(ShakeText(debufText.transform, 0.1f, 10f, cts.Token));
                debufText.text = "Control Inverted!";
                break;
            case DebufType.Slip:
                StartCoroutine(PulseText(debufText.transform, 1f, 1.2f, 2f, cts.Token));
                debufText.text = "Slipped!";
                break;
            case DebufType.Mosquito:
                StartCoroutine(ShakeText(debufText.transform, 0.1f, 10f, cts.Token));
                debufText.text = "Mosquito Bite!";
                break;
            case DebufType.CameraTilt:
                StartCoroutine(ShakeText(debufText.transform, 0.1f, 8f, cts.Token));
                debufText.text = "Camera Tilted!";
                break;
            case DebufType.CameraFlip:
                StartCoroutine(PulseText(debufText.transform, 1f, 1.15f, 1.5f, cts.Token));
                debufText.text = "Camera Flipped!";
                break;
            default:
                if (cts != null)
                {
                    cts.Cancel(); // Cancel any ongoing effects
                    cts = new CancellationTokenSource(); // Create a new token source for future use
                }
                debufText.text = "";
                break;
        }
    }

    private IEnumerator PulseText(Transform target, float minScale, float maxScale, float speed, CancellationToken token)
    {
        Vector3 origScale = target.localScale;
        float t = 0f;
        while (!token.IsCancellationRequested)
        {
            t += Time.deltaTime * speed;
            float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(t, 1f));
            target.localScale = origScale * scale;
            yield return null;
        }
        target.localScale = origScale;
    }

    private IEnumerator ShakeText(Transform target, float magnitude, float speed, CancellationToken token)
    {
        Vector3 origPos = target.localPosition;
        float t = 0f;
        while (!token.IsCancellationRequested)
        {
            t += Time.deltaTime * speed;
            float offsetX = Mathf.Sin(t * Mathf.PI * 2f) * magnitude;
            target.localPosition = origPos + new Vector3(offsetX, 0f, 0f);
            yield return null;
        }
        target.localPosition = origPos;
    }
}