using UnityEngine;
using UnityEngine.UI;

public class DeathbringerManager : MonoBehaviour
{
    [Header("Deathbringer Settings")]
    public GameObject deathbringerPrefab;
    public Transform spawnPoint;

    [Header("UI")]
    public Slider summonSlider;
    public Image fillImage; // Fill 이미지 참조

    [Header("Summon Settings")]
    public float initialSummonTime = 100f;
    private float remainingTime;

    private bool hasSummoned = false;
    private GameObject spawnedDeathbringer;

    [Header("Colors")]
    public Color timerColor = Color.cyan; // 타이머용 파란색
    public Color healthColor = Color.red; // 체력바용 빨간색

    EnemyHealth deathbringerHealth;

    void Start()
    {
        remainingTime = initialSummonTime;
        UpdateSlider();
        SetFillColor(timerColor); // 시작 시 타이머 색상
    }

    void Update()
    {

        remainingTime -= Time.deltaTime;
        UpdateSlider();

        if (remainingTime <= 0f && !hasSummoned)
        {
            SummonDeathbringer();
        }
    }

    void UpdateSlider()
    {
        if (summonSlider != null && !hasSummoned)
        {
            summonSlider.value = remainingTime;
        }
        else
        {
            summonSlider.value = deathbringerHealth.enemyHealth;
        }
    }

    public void OnSlimeDied()
    {
        if (!hasSummoned)
        {
            remainingTime -= 1f;
        }
    }

    void SummonDeathbringer()
    {
        hasSummoned = true;
        spawnedDeathbringer = Instantiate(deathbringerPrefab, spawnPoint.position, Quaternion.identity);

        // 슬라이더 설정을 체력바로 전환
        deathbringerHealth = spawnedDeathbringer.GetComponent<EnemyHealth>();
        if (deathbringerHealth != null && summonSlider != null)
        {
            SetFillColor(healthColor); // 색상 전환

            summonSlider.value = deathbringerHealth.enemyHealth;

        }
    }

    void SetFillColor(Color color)
    {
        if (fillImage != null)
        {
            fillImage.color = color;
        }
    }
}
