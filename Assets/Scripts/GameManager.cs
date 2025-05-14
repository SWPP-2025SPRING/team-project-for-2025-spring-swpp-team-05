using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Inspector: connect
    public PlayerControl playerControl;
    // public UIManager uiManager;


    public bool isGameActive;    
    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
