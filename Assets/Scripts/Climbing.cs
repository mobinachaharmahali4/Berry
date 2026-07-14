using UnityEngine;
using UnityEngine.InputSystem;

public class Climbing : MonoBehaviour
{
    public float climbSpeed = 4f;

    private bool onLadder = false;
    private float climbMinY;
    private float climbMaxY;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!onLadder) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.upArrowKey.isPressed || keyboard.wKey.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbSpeed);
        }
        else if (keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -climbSpeed);
        }
        else
        {
            // وقتی دست از کیبورد برداشت وایسته
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }

        // محدود کردن به ارتفاع نردبون
        float clampedY = Mathf.Clamp(transform.position.y, climbMinY, climbMaxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("برخورد با: " + other.tag);

        if (other.tag == "Climb")
        {
            Bounds ladderBounds = other.bounds;
            climbMinY = ladderBounds.min.y;
            climbMaxY = ladderBounds.max.y;

            rb.gravityScale = 0f;
            onLadder = true;
        }
    }
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Climb")
        {
            Bounds ladderBounds = collision.collider.bounds;
            climbMinY = ladderBounds.min.y;
            climbMaxY = ladderBounds.max.y;

            // گرانش رو خاموش کن وگرنه پلیر میفته پایین
            rb.gravityScale = 0f;
            onLadder = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Climb")
        {
            // گرانش رو دوباره روشن کن
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            onLadder = false;
        }
    }

    // موبایل
    public void ClimbUp()   => StartCoroutine(MobileClimb(1f));
    public void ClimbDown() => StartCoroutine(MobileClimb(-1f));

    private System.Collections.IEnumerator MobileClimb(float direction)
    {
        while (onLadder)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbSpeed * direction);
            yield return null;
        }
    }
}