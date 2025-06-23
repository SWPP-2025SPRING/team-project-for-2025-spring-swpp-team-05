using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Key
}

public class PlayerItemManager : MonoBehaviour
{
    public AudioClip keySound; // Sound played when the player collects a key
    List<ItemType> collectedItems = new List<ItemType>();
    // Start is called before the first frame update
    void Start()
    {
        collectedItems.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool HasItem(ItemType itemType)
    {
        return collectedItems.Contains(itemType);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PlayerItemManager OnTriggerEnter: " + other.name);
        if (other.CompareTag("Key"))
        {
            // 키 아이템 획득
            ItemType itemType = ItemType.Key;
            if (!collectedItems.Contains(itemType))
            {
                collectedItems.Add(itemType);
                SoundEffectManager.Instance.PlayOneShotOnce(keySound);
                TitleManager.Instance.ShowEventText("어딘가의 열쇠를 획득하였다...", Color.white, FlashPreset.Dramatic);
                Debug.Log("Key collected!");
                Destroy(other.gameObject); // 키 오브젝트 삭제
            }
        }
    }
}
