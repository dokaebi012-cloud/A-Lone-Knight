using UnityEngine;

public class EnemyRotationManager : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        //플레이어가 객체의 오른쪽에 있으면 오른쪽을 바라본다
        if((player.transform.position.x - transform.position.x) > 0)
        {
            //transform.Rotate(0, 180, 0);  //상대 회전, 객체 기준 회전값 누적
            //플레이어와의 상대적인 값 변동시 매우 빠르게 제자리 선회

            transform.rotation = Quaternion.Euler(0, 180, 0); //절대 회전, 월드 기준 회전값 설정, 누적 없음
        }
        else
        {
            //transform.Rotate(0, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
