using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorKey : MonoBehaviour
{

    public AudioClip doorOpenSound; // 문 열리는 소리
    public AudioClip doorRejectSound; // 문 열리지 않는 소리

    public GameObject door1; // 엘리베이터 문1
    public GameObject door2; // 엘리베이터 문2
    public float openSpeed = 2f; // 문 열리는 속도
    public float openDistance = 3f; // 문이 열리는 거리
    public float closeSpeed = 2f; // 문 닫히는 속도
    public float closeDistance = 0f; // 문이 닫히는 거리
    private Vector3 door1InitialPosition;
    private Vector3 door2InitialPosition;
    private bool isOpen = false; // 문이 열렸는지 여부
    private bool isClosing = false; // 문이 닫히는 중인지 여부
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerItemManager itemManager = other.GetComponent<PlayerItemManager>();

            // 키가 없을떄
            if (itemManager != null && !itemManager.HasItem(ItemType.Key))
            {
                SoundEffectManager.Instance.PlayOneShotOnce(doorRejectSound); // 문 열리지 않는 소리 재생
                TitleManager.Instance.ShowEventText("이 엘리베이터를 타려면 열쇠가 필요해보인다...", Color.white, FlashPreset.Dramatic);
            }
            else
            {
                TitleManager.Instance.ShowEventText("엘리베이터를 타고 올라간다...", Color.white, FlashPreset.Dramatic);
                if (!isOpen && !isClosing)
                {
                    StartCoroutine(OpenDoors());
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOpen)
            {
                StartCoroutine(CloseDoors());
            }
        }
    }

    IEnumerator OpenDoors()
    {
        if (isOpen || isClosing) yield break; // 이미 열려있거나 닫히는 중이면 중단

        isOpen = true;
        door1InitialPosition = door1.transform.position;
        door2InitialPosition = door2.transform.position;

        Vector3 targetPosition1 = door1InitialPosition + Vector3.right * openDistance;
        Vector3 targetPosition2 = door2InitialPosition + Vector3.left * openDistance;
        SoundEffectManager.Instance.PlayOneShotOnce(doorOpenSound); // 문 열리는 소리 재생

        while (Vector3.Distance(door1.transform.position, targetPosition1) > 0.01f ||
               Vector3.Distance(door2.transform.position, targetPosition2) > 0.01f)
        {
            door1.transform.position = Vector3.MoveTowards(door1.transform.position, targetPosition1, openSpeed * Time.deltaTime);
            door2.transform.position = Vector3.MoveTowards(door2.transform.position, targetPosition2, openSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator CloseDoors()
    {
        if (!isOpen || isClosing) yield break; // 이미 닫혀있거나 열리는 중이면 중단

        isClosing = true;

        Vector3 targetPosition1 = door1InitialPosition + Vector3.right * closeDistance;
        Vector3 targetPosition2 = door2InitialPosition + Vector3.left * closeDistance;

        while (Vector3.Distance(door1.transform.position, targetPosition1) > 0.01f ||
               Vector3.Distance(door2.transform.position, targetPosition2) > 0.01f)
        {
            door1.transform.position = Vector3.MoveTowards(door1.transform.position, targetPosition1, closeSpeed * Time.deltaTime);
            door2.transform.position = Vector3.MoveTowards(door2.transform.position, targetPosition2, closeSpeed * Time.deltaTime);
            yield return null;
        }

        isOpen = false;
        isClosing = false;
    }


}
