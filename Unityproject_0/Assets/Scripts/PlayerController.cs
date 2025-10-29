using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;

    [Header("점프 설정")]
    public float jumpForce = 10.0f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded = false;  // 바닥에 닿아있는지 여부
    //private int score = 0;
    private Vector3 startPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D가 없습니다!");
        }

        startPosition = transform.position;
        Debug.Log("시작 위치 저장: " + startPosition);

    }

    void Update()
    {
        // 좌우 이동 입력
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            spriteRenderer.flipX = true; // 왼쪽으로 이동 시 이미지 좌우 반전
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
            spriteRenderer.flipX = false; // 오른쪽으로 이동 시 이미지 좌우 반전
        }

        // 물리 기반 이동
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // 점프 입력
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            Debug.Log("점프!");
            animator.SetTrigger("Jump");
        }

        // 애니메이션
        float currentSpeed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", currentSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        // 장애물 충돌 시 생명 감소로 변경!
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 생명 -1");
            // GameManager 찾아서 생명 감소
            GameManager gameManager = FindObjectOfType<GameManager>();

            if (gameManager != null)
            {
                gameManager.TakeDamage(1);  // 생명 1 감소
            }

            // 짧은 무적 시간 (0.5초 후 원래 위치로)
            transform.position = startPosition;
            rb.velocity = Vector2.zero;
        }
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에서 떨어짐");
            isGrounded = false;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // 코인 수집 (기존 코드)
        //if (other.CompareTag("Coin"))
        //{
        //    score += 1;
        //    Debug.Log("💰 코인 획득! 현재 점수: " + score);
        //    Destroy(other.gameObject);
        //}


        // 골 도달 - 새로 추가!
        if (other.CompareTag("Goal"))
        {
            Debug.Log("🎉 Goal Reached!");
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameClear();  // 게임 클리어 함수 호출
            }
        }
    }
}

