using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f;  // سرعت شتاب گرفتن
    public float deceleration = 15f;  // سرعت وایستادن

    private float moveDirection = 0f;
    private float currentSpeed = 0f;  // سرعت فعلی

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
                moveDirection = -1f;
            else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
                moveDirection = 1f;
        }

        if (moveDirection != 0f)
        {
            // شتاب گرفتن
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveDirection * moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // کم کم وایستادن
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);

        // ریست direction برای موبایل
        moveDirection = 0f;
    }

    public void MoveLeft()   => moveDirection = -1f;
    public void MoveRight()  => moveDirection = 1f;
    public void StopMoving() => moveDirection = 0f;
}