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
        if (!onLadder)
            return;

        var keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.upArrowKey.isPressed || keyboard.wKey.isPressed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                climbSpeed
            );
        }
        else if (keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                -climbSpeed
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                0f
            );
        }

        float clampedY = Mathf.Clamp(
            transform.position.y,
            climbMinY,
            climbMaxY
        );

        transform.position = new Vector3(
            transform.position.x,
            clampedY,
            transform.position.z
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Climb"))
        {
            Debug.Log("on triger");

            Bounds ladderBounds = other.bounds;

            climbMinY = ladderBounds.min.y;
            climbMaxY = ladderBounds.max.y;

            onLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Climb"))
        {
            Debug.Log("off triger");

            onLadder = false;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                0f
            );
        }
    }

    // موبایل
    public void ClimbUp()
    {
        StartCoroutine(MobileClimb(1f));
    }

    public void ClimbDown()
    {
        StartCoroutine(MobileClimb(-1f));
    }

    private System.Collections.IEnumerator MobileClimb(float direction)
    {
        while (onLadder)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                climbSpeed * direction
            );

            yield return null;
        }
    }
}