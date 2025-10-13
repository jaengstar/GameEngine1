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
    private int score = 0;
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
        }
        
        // 애니메이션
        float currentSpeed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", currentSpeed);
    }

   void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 "Ground" Tag를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에 착지!");
            isGrounded = true;
        }
        // 장애물 충돌 감지 - 새로 추가!
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 시작 지점으로 돌아갑니다.");
        
        // 시작 위치로 순간이동
            transform.position = startPosition;
        
        // 속도 초기화 (안 하면 계속 날아감)
            rb.velocity = new Vector2(0,0);
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
    if (other.CompareTag("Coin"))
    {
        score += 1;
        Debug.Log("💰 코인 획득! 현재 점수: " + score);
        Destroy(other.gameObject);
    }
    
    // // 별 수집 (기존 코드)
    // if (other.CompareTag("Star"))
    // {
    //     score += 5;
    //     Debug.Log("⭐ 별 획득! +5점! 현재 점수: " + score);
    //     Destroy(other.gameObject);
    // }
    
    // 골 도달 - 새로 추가!
    if (other.CompareTag("Goal"))
    {
        Debug.Log("🎉🎉🎉 게임 클리어! 🎉🎉🎉");
        Debug.Log("최종 점수: " + score + "점");
        
        // 캐릭터 조작 비활성화
        enabled = false;
    }
    }
}

