using UnityEngine;

/// <summary>
/// Basic platformer player with simple movement, jump and projectile logic.
/// </summary>
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Transform[] groundPoints;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool airControl = true;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private GameObject hadoukenPrefab;

    private Animator animator;
    public Rigidbody2D Body { get; private set; }
    private bool facingRight = true;

    public bool Attack { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        OnGround = IsGrounded();
        HandleMovement(horizontal);
        Flip(horizontal);
        HandleLayers();
    }

    private void HandleMovement(float horizontal)
    {
        if (Body.velocity.y < 0f)
        {
            animator.SetBool("land", true);
        }

        if (!Attack && (OnGround || airControl))
        {
            Body.velocity = new Vector2(horizontal * movementSpeed, Body.velocity.y);
        }

        if (Jump && Mathf.Abs(Body.velocity.y) < 0.01f)
        {
            Body.AddForce(new Vector2(0f, jumpForce));
        }

        animator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("uppercut");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger("throw");
            ThrowHadouken();
        }
    }

    private void Flip(float horizontal)
    {
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private bool IsGrounded()
    {
        if (Body.velocity.y <= 0f)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, groundMask);
                foreach (var collider in colliders)
                {
                    if (collider.gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        animator.SetLayerWeight(1, OnGround ? 0 : 1);
    }

    public void ThrowHadouken()
    {
        Vector3 rotation = facingRight ? Vector3.zero : new Vector3(0f, 0f, 180f);
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        GameObject projectile = Instantiate(hadoukenPrefab, transform.position, Quaternion.Euler(rotation));
        projectile.GetComponent<Hadouken>().Initialize(direction);
    }
}
