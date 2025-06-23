using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed = 5f; // 회전 속도
    public float floatFrequency = 1f; // 부유 주기
    public float floatAmplitude = 0.1f; // 부유 진폭

    private Vector3 initialPosition;
    void Start()
    {
        initialPosition = transform.position; // 초기 위치 저장
    }

    // Update is called once per frame
    void Update()
    {
        // 부드러운 위아래 움직임
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

        // 부드러운 회전
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
