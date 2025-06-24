using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CamPoint
{
    public Vector3 position;
    public Quaternion rotation;
    public float duration; // ≤0 이면 defaultDuration 사용
}

public class CinematicCamera : MonoBehaviour
{
    public static CinematicCamera Instance { get; private set; }
    [Header("Cinematic Points")]
    public CamPoint[] points;

    [Header("Settings")]
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float defaultDuration = 3f;

    void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator StartCinematic(IEnumerator cinematicCoroutine)
    {
        // Disable player control
        GameManager.Instance.StopTimeTick();

        // Start the cinematic coroutine
        yield return StartCoroutine(cinematicCoroutine);

        GameManager.Instance.ResumeTimeTick(); // Resume game time after cinematic
    }

    public IEnumerator FocusCinematic(GameObject target, Vector3 position, float duration1 = 1f, float duration2 = 1f, IEnumerator eventCoroutine = null)
    {
        transform.GetPositionAndRotation(out Vector3 initialPosition, out Quaternion initialRotation);
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - position, Vector3.up);

        float elapsedTime = 0f;
        while (elapsedTime < duration1)
        {
            float t = elapsedTime / duration1;
            transform.position = Vector3.Lerp(initialPosition, position, t);
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        if (eventCoroutine != null)
        {
            yield return StartCoroutine(eventCoroutine);
        }
        else
        {
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration2)
        {
            float t = elapsedTime / duration2;
            transform.position = Vector3.Lerp(position, initialPosition, t);
            transform.rotation = Quaternion.Slerp(targetRotation, initialRotation, t);
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
    }


    /**
    * Function for initial cinematic camera setup.
    */

    public void StartCinematic()
    {
        GameManager.Instance.StopTimeTick(); // Stop game time for cinematic
        StartCoroutine(PlayCinematic());
    }


    private IEnumerator PlayCinematic()
    {
        if (points == null || points.Length == 0) yield break;

        Transform camT = transform;
        // 첫 포인트로 즉시 세팅
        camT.position = points[0].position;
        camT.rotation = points[0].rotation;
        yield return null;

        // 나머지 구간 보간
        for (int i = 1; i < points.Length; i++)
        {
            Vector3 startPos = camT.position;
            Quaternion startRot = camT.rotation;
            Vector3 endPos = points[i].position;
            Quaternion endRot = points[i].rotation;
            float dur = points[i].duration > 0 ? points[i].duration : defaultDuration;

            float elapsed = 0f;
            while (elapsed < dur)
            {
                float t = easeCurve.Evaluate(elapsed / dur);
                camT.position = Vector3.Lerp(startPos, endPos, t);
                camT.rotation = Quaternion.Slerp(startRot, endRot, t);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            camT.position = endPos;
            camT.rotation = endRot;
        }

        GameManager.Instance.ResumeTimeTick(); // Resume game time after cinematic
        // (끝나고 다른 로직이 필요하면 여기서 호출)
    }
}
