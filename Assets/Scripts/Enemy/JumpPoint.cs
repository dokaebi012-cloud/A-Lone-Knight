using UnityEngine;

public class JumpPoint : MonoBehaviour
{
    public float jumpForce = 7f;
    public float horizontalForce = 2f;

    // 점프를 허용할 접근 방향
    public enum AllowedDirection { Both, LeftOnly, RightOnly }
    public AllowedDirection allowedDirection = AllowedDirection.Both;
}
