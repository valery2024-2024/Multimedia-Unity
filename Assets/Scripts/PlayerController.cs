using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 13f;

    [Header("Refs")]
    [SerializeField] private SpriteRenderer spriteRenderer; // перетягни або знайдеться автоматично
    [SerializeField] private Animator anim;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float inputX;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!anim) anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Зчитуємо напрямок руху (A/D або ←/→)
        inputX = Input.GetAxisRaw("Horizontal");

        // Перевертаємо спрайт у потрібний бік
        if (inputX != 0)
            spriteRenderer.flipX = inputX < 0;

        // Стрибок, тільки якщо стоїмо на землі
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }

        // Передаємо параметри в Animator
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("IsGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        // Рух по осі X
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);

        // Перевірка землі
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Візуалізація точки перевірки землі
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
