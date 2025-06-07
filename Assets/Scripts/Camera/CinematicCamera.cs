using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    public static CinematicCamera Instance { get; private set; }

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

        StartCoroutine(eventCoroutine);

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
}
