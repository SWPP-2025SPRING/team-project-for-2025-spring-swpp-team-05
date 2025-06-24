using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public AudioClip goalSound; // GoalTrigger에 닿았을 때 재생할 소리
    public GameObject player;
    public GameObject playerEnd;

    private bool isTriggered = false; // GoalTrigger가 트리거되었는지 여부
    // Start is called before the first frame update
    void Start()
    {
        playerEnd.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            if (collision.transform.position.y < transform.position.y)
            {
                return; // 플레이어가 GoalTrigger보다 아래에 있을 때는 무시
            }
            // 플레이어가 GoalTrigger에 닿았을 때
            isTriggered = true; // GoalTrigger가 트리거되었음을 표시
            Debug.Log("Goal Triggered!");
            StartCoroutine(EndSceneCoroutine());
        }
    }

    IEnumerator EndSceneCoroutine()
    {
        TitleManager.Instance.ShowEndTitle("Clear!", Color.white, FlashPreset.Dramatic);
        SoundEffectManager.Instance.PlayOneShotOnce(goalSound); // GoalTrigger에 닿았을 때 소리 재생
        BGMManager.Instance.StopBGM(); // 현재 BGM 정지
        yield return new WaitForSeconds(2f); // 2초 대기
        BGMManager.Instance.PlayFieldBGM(); // 엔딩 BGM 재생
        float spa = PlayerStatus.instance.spa;
        GradeResult gradeResult = GradeResult.GetString(spa);
        TitleManager.Instance.ShowEndSubtitle($"Final Grade : {spa:F2} ({gradeResult.Grade})", Color.white, FlashPreset.Dramatic);
        FollowPlayer followPlayer = Camera.main.GetComponent<FollowPlayer>();
        yield return new WaitForSeconds(3f); // 2초 대기
        followPlayer.SetEndScene(playerEnd);
        player.SetActive(false); // 플레이어 비활성화
        playerEnd.SetActive(true); // 엔드 씬 플레이어 활성화
    }
}
