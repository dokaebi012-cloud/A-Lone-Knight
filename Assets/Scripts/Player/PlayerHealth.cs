using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;
    public int shieldCount = 0;

    public TextMeshProUGUI shieldCountUI;

    public void CheckShieldCount()
    {
        shieldCountUI.text = shieldCount.ToString();
    }
}
