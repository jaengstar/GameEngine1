using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 10;  // ���� ���� (10��)

    void OnTriggerEnter2D(Collider2D other)
    {
        // Player�� �浹�ߴ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            // GameManager ã��
            GameManager gameManager = FindObjectOfType<GameManager>();

            if (gameManager != null)
            {
                gameManager.AddScore(coinValue);  // ���� ����
            }

            Destroy(gameObject);  // ���� ����
        }
    }
}