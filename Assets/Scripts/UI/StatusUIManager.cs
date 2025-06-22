using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUIManager : MonoBehaviour
{
    public float xOffset = 100.0f;
    public float duration = 1.0f;
    public float intduration = 0.5f;

    public TextMeshProUGUI[] statusBeforeTexts;
    public TextMeshProUGUI[] statusAfterTexts;
    public GameObject statusInt;

    public static StatusUIManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HideTexts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowStatusUpdate(List<float> statusBefore, List<float> statusAfter)
    {
        StartCoroutine(UpdateStatusValue(statusBefore, statusAfter));
    }

    IEnumerator UpdateStatusValue(List<float> statusBefore, List<float> statusAfter)
    {
        if (statusBefore.Count != statusAfter.Count || statusBefore.Count != statusBeforeTexts.Length)
        {
            Debug.LogError("Status lists do not match the number of UI elements.");
            yield break;
        }
        UpdateBeforeStatus(statusBefore);
        yield return StartCoroutine(MoveLeft());
        yield return new WaitForSeconds(intduration);
        statusInt.SetActive(true);
        yield return new WaitForSeconds(intduration);
        UpdateAfterStatus(statusAfter);
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(MoveRight());
        HideTexts();
    }

    void UpdateBeforeStatus(List<float> statusBefore)
    {
        if (statusBefore.Count != statusBeforeTexts.Length)
        {
            Debug.LogError("Status before list does not match the number of UI elements.");
            return;
        }

        for (int i = 0; i < statusBefore.Count; i++)
        {
            statusBeforeTexts[i].gameObject.SetActive(true);
            statusBeforeTexts[i].text = statusBefore[i].ToString("F2");
        }
    }

    void UpdateAfterStatus(List<float> statusAfter)
    {
        if (statusAfter.Count != statusAfterTexts.Length)
        {
            Debug.LogError("Status after list does not match the number of UI elements.");
            return;
        }

        for (int i = 0; i < statusAfter.Count; i++)
        {
            statusAfterTexts[i].gameObject.SetActive(true);
            statusAfterTexts[i].text = statusAfter[i].ToString("F2");
        }
    }

    void HideTexts()
    {
        foreach (var text in statusBeforeTexts)
        {
            text.gameObject.SetActive(false);
        }
        foreach (var text in statusAfterTexts)
        {
            text.gameObject.SetActive(false);
        }
        statusInt.SetActive(false);
    }

    IEnumerator MoveRight()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(xOffset, 0, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    IEnumerator MoveLeft()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition - new Vector3(xOffset, 0, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
