using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Hadouken : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 direction = Vector2.right;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    public void Initialize(Vector2 dir)
    {
        direction = dir;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
