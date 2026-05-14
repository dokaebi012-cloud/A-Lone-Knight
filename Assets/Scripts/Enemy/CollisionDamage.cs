using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    public float damage = 20f;
    public float damageInterval = 1f; // 피해 간격 (초)
    public bool isActive = true;

    // 여러 객체에 각기 다른 피해 인터벌을 적용하기 위해 dictionary<충돌체, 마지막 충돌 시점> 사용
    private Dictionary<Collider2D, float> lastDamageTime = new Dictionary<Collider2D, float>();

    // 충돌을 지속하는 동안 플레이어에게 데미지를 주는 함수
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(gameObject.name + " collided with " + collision.name);
        if (collision.CompareTag("Player") && isActive)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // 처음 충돌한 경우 딕셔너리에 콜라이더 등록
                if (!lastDamageTime.ContainsKey(collision))
                {
                    lastDamageTime[collision] = Time.time;
                    playerHealth.GetDamage(damage);
                }

                // 초기 시간값 설정
                float timeSinceLastDamage = Time.time - lastDamageTime[collision];
                // 현재 시간과 마지막 피해 시간의 차이 계산, 충돌 후 시간이 인터벌을 넘었을 경우
                if (timeSinceLastDamage >= damageInterval)
                {
                    // 피해 계산 및 마지막 피해 시간 업데이트
                    playerHealth.GetDamage(damage);
                    lastDamageTime[collision] = Time.time;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (lastDamageTime.ContainsKey(collision))
        {
            lastDamageTime.Remove(collision); // 충돌 종료 시 딕셔너리 정리
        }
    }

}
