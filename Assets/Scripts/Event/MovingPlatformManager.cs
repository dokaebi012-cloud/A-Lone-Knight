using UnityEngine;

public class MovingPlatformManager : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxDistance = 3.0f;
    private Vector3 startPos;
    private int direction = 1;
    public MovingPlatformType movingPlatformType;
    private float movedDistance;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        movedDistance = speed * Time.deltaTime;

        if (movingPlatformType == MovingPlatformType.Up)
        {
            if ((startPos.y + maxDistance) - (transform.position.y + movedDistance) > 0)
            {
                transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            }
            else
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            }
        }
        else if (movingPlatformType == MovingPlatformType.Right)
        {
            if (transform.position.x > startPos.x + maxDistance)
            {
                direction = -1;
            }
            else if (transform.position.x < startPos.x - maxDistance)
            {
                direction = 1;
            }

            transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);

        }

        // (startPos.x - maxDistance) --- startPos.x --- (startPos.x + maxDistance)
        // 일단 정해진 방향으로 이동한 다음 최대 변위를 초과하면 방향을 바꾼다


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(gameObject.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.SetParent(null);
    }
}

public enum MovingPlatformType
{
    Up,
    Right,
    //Left,
    //Down
}
