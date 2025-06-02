using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Inspector: connect
    public PlayerControl playerControl;
    public UIManager uiManager;
    // Game Stat
    public bool isGameActive = true;
    private float elapsedTime = 0f;

    public bool isGameActive;
    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            elapsedTime += Time.deltaTime;

            float speed = PlayerStatus.instance.moveSpeed * 3.6f;
            float atkRatio = PlayerStatus.instance.GetAttackPowerRatio();
            float rangeRatio = PlayerStatus.instance.GetAttackRangeRatio();

            uiManager.UpdateTimer(elapsedTime);
            uiManager.UpdateStats(speed, atkRatio, rangeRatio);
        }
    }
}