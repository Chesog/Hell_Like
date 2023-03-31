using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class Character_Input : MonoBehaviour
{
    [Header("SetUp")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform feet_Pivot;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferTimeCounter;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    private Coroutine _jumpCorutine;
    [Header("Movement")]
    [SerializeField] Vector3 _CurrentMovement;
    [Range(0, 500)] [SerializeField] private float speed = 20.0f;
    [SerializeField] float initialSpeed;
    [Range(0, 500)] [SerializeField] private float jumpForce = 20.0f;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isSprinting;
    [SerializeField] const float maxDistance = 10f;
    [SerializeField] const float minJumpDistance = 0.5f;
    [Header("Coyote Time Setup")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimerCounter;

    private void Awake()
    {
        rigidbody ??= GetComponent<Rigidbody>();
        if (!rigidbody)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(rigidbody)} is null");
            enabled = false;
        }

        feet_Pivot ??= GetComponent<Transform>();
        if (!feet_Pivot)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(feet_Pivot)} is null");
        }

        playerCamera ??= GetComponent<Transform>();
        if (!feet_Pivot)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(playerCamera)} is null");
        }

        isJumping = false;
        isSprinting = false;
        initialSpeed = speed;
    }

    private void FixedUpdate()
    {
        //Debug.Log(isGrounded());

        if (isGrounded())
        {
            coyoteTimerCounter = coyoteTime;
        }
        else
        {
            coyoteTimerCounter -= Time.deltaTime;
        }

        if (isGrounded())
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
            //coyoteTimerCounter = 0.0f;
        }

        // To Fix

        if (_CurrentMovement.magnitude >= 1f)
        {
            float targetAngle = Mathf.Atan2(_CurrentMovement.x, _CurrentMovement.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        
        
            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            rigidbody.velocity = moveDir.normalized * speed + Vector3.up * rigidbody.velocity.y;
        }

        //rigidbody.velocity = _CurrentMovement * speed + Vector3.up * rigidbody.velocity.y;

        if (isSprinting)
        {
            speed = initialSpeed * 2;
        }
        else
        {
            speed = initialSpeed;
        }
    }

    public void OnMove(InputValue input)
    {
        var movement = input.Get<Vector2>();
        _CurrentMovement = new Vector3(movement.x, 0f, movement.y);
        //if (movement.x != 0 && movement.y != 0)
        //{
        //    _CurrentMovement = new Vector3(movement.x, 0f, movement.y);
        //}
        //else
        //{
        //    rigidbody.velocity = new Vector3(0f,0f,0f);
        //}
    }

    public void OnJump(InputValue input)
    {
        if (_jumpCorutine != null)
            StopCoroutine(_jumpCorutine);
        _jumpCorutine = StartCoroutine(JumpCorutine(jumpBufferTime));

        StopCoroutine(JumpCorutine(jumpBufferTime));
        if (input.isPressed && rigidbody.velocity.y > 0f)
        {
            rigidbody.velocity = _CurrentMovement * speed + Vector3.up * rigidbody.velocity.y * 0.5f;
            coyoteTimerCounter = 0f;
        }
    }

    private IEnumerator JumpCorutine(float bufferTime)
    {
        if (!feet_Pivot)
        {
            yield break;
        }

        float timeElapsed = 0;

        while (timeElapsed <= bufferTime)
        {
            yield return new WaitForFixedUpdate();

            if (coyoteTimerCounter > 0f && jumpBufferTimeCounter > 0f && !isJumping)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                if (timeElapsed > 0)
                {
                    Debug.Log(message: $"{name}: buffer jump for {timeElapsed} seconds");
                }
                yield break;

            }

            timeElapsed += Time.fixedDeltaTime;
        }
    }

    private bool isGrounded()
    {
        return Physics.Raycast(feet_Pivot.position, Vector3.down, out var hit, maxDistance) && hit.distance <= minJumpDistance;
    }

    public void OnSprint(InputValue input)
    {
        Debug.Log(input.isPressed);
        isSprinting = input.isPressed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(feet_Pivot.position, feet_Pivot.position + Vector3.down * minJumpDistance);
    }
}
