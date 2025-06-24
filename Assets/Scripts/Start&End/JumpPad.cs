using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public AudioClip jumpSound; // JumpPad에 닿았을 때 재생할 소리
    public float jumpForce = 50f; // JumpPad의 점프 힘
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                SoundEffectManager.Instance.PlayOneShotOnce(jumpSound); // JumpPad에 닿았을 때 소리 재생
                Vector3 jumpForceV = new Vector3(0, jumpForce, 0); // Adjust the jump force as needed
                playerRigidbody.AddForce(jumpForceV, ForceMode.Impulse);
            }
        }
    }
}
