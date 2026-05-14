using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    public GameObject itemPrefab; // 드랍할 아이템 프리팹

    [Range(0f, 1f)] 
    public float dropRate;        // 개별 아이템 드랍 확률 (0~1)
}

public class EnemyItemDropper : MonoBehaviour
{
    [Header("Drop Settings")]
    //[Range(0f, 1f)]
    //public float dropChance = 0.5f;         // 아이템 드랍 확률

    public DropItem[] dropItems;           // 드랍 가능한 아이템 목록

    private EnemyHealth enemyHealth;

    private GameObject itemWillDrop;
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += HandleDrop;  // 몬스터 사망 이벤트 구독
        }
    }

    void HandleDrop()
    {
        //if (Random.value > dropChance) return;

        // 드랍 가능한 후보 리스트 구성
        List<DropItem> possibleDrops = new List<DropItem>();

        foreach (var dropItem in dropItems)
        {
            if (dropItem.itemPrefab != null && Random.value <= dropItem.dropRate)
            {
                possibleDrops.Add(dropItem);
            }
        }

        // 후보가 존재하면 무작위로 하나 선택해 드랍
        if (possibleDrops.Count > 0)
        {
            itemWillDrop = possibleDrops[Random.Range(0, possibleDrops.Count)].itemPrefab;
            Instantiate(itemWillDrop, transform.position, Quaternion.identity);

        }
    }

    void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= HandleDrop; // 이벤트 해제
        }
    }
}
