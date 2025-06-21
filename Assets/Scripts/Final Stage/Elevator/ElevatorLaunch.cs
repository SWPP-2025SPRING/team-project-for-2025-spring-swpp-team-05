using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLaunch : MonoBehaviour
{
    // Start is called before the first frame update
    public float waitTime = 5f;
    public float riseHeight = 150f;
    public float targetHeight = 100f;
    public float riseDuration = 2f;
    public float fallDuration = 3f;

    private bool hasActivated = false;

    public Transform elevator;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (hasActivated) return;

        if (other.CompareTag("Player"))
        {
            PlayerItemManager itemManager = other.GetComponent<PlayerItemManager>();

            if (itemManager != null && !itemManager.HasItem(ItemType.Key))
            {
                TitleManager.Instance.ShowEventText("이 엘리베이터를 타려면 열쇠가 필요해보인다...", Color.white, FlashPreset.Dramatic);
            }
            else
            {
                TitleManager.Instance.ShowEventText("5초뒤 엘리베이터가 출발합니다.", Color.white, FlashPreset.Dramatic);
                hasActivated = true;
                PlayerStatus.instance.StopPlayer();

                // 플레이어를 엘리베이터에 태움
                other.transform.SetParent(elevator);
                StartCoroutine(MoveElevator());
            }
        }
    }

    IEnumerator MoveElevator()
    {
        yield return new WaitForSeconds(waitTime);
        TitleManager.Instance.ShowEventText("발사!", Color.white, FlashPreset.Dramatic);

        Vector3 startPos = elevator.position;
        Vector3 peakPos = startPos + Vector3.up * riseHeight;
        Vector3 finalPos = startPos + Vector3.up * targetHeight;

        // 빠르게 상승 (2초)
        float t = 0f;
        while (t < riseDuration)
        {
            t += Time.deltaTime;
            float lerp = t / riseDuration;
            elevator.position = Vector3.Lerp(startPos, peakPos, lerp);
            yield return null;
        }

        // 천천히 하강 (3초)
        t = 0f;
        while (t < fallDuration)
        {
            t += Time.deltaTime;
            float lerp = t / fallDuration;
            elevator.position = Vector3.Lerp(peakPos, finalPos, lerp);
            yield return null;
        }

        // 도착 후 플레이어 분리 (원하는 타이밍에)
        if (elevator.childCount > 0)
        {
            Transform player = elevator.GetChild(0);
            player.SetParent(null);
            PlayerStatus.instance.ResumePlayer(); // 플레이어 상태 재개
        }
    }
}
