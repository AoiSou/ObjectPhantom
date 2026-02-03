using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 45.0f;
    public float movespeed;
    public float walkspeed;
    public float sprintspeed;
    public float wallrunSpeed;
    public float swingspeed;
    
    public float groundDrag=5f;
    
    [Header("Jump")]
    public float jumpForce=12f;
    public float jumpCooldown=0.25f;
    public float airMultiplier=0.4f;
    bool readyToJump=true;
    
    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    
    [Header("Keybinds")]
    public KeyCode jumpKey=KeyCode.Space;
    public KeyCode sprintKey=KeyCode.LeftShift;
    public KeyCode crouchKey=KeyCode.LeftControl;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    
    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    
    [Header("Camera Effects")]
    public PlayerCam cam;
    public float grappleFov = 95f;

    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;
    
    Vector3 moveDirection;
    
    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        swinging,
        crouching,
        freeze,
        air
    }

    public bool freeze;
    public bool activeGrapple;
    public bool wallrunning;
    public bool swinging;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        readyToJump=true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        //Debug.Log(movespeed);
        
        grounded= Physics.Raycast(transform.position,Vector3.down,playerHeight*0.5f+0.2f, whatIsGround);
        
        MyInput();
        SpeedControl();
        StateHandler();
        
        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        //jump
        if (Input.GetKey(jumpKey)&&readyToJump&&grounded)
        {
            readyToJump = false;
            
            Jump();
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        //crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    void StateHandler()
    {
        if(freeze)
        {
           state = MovementState.freeze;
         movespeed = 0f;
        rb.velocity = Vector3.zero;
        }
        
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            movespeed = crouchSpeed;
        }
        
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            movespeed = wallrunSpeed;
        }
        
        if (swinging)
        {
            state=MovementState.swinging;
            movespeed = swingspeed;
        }
        
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            movespeed = sprintspeed;
        }
        
        else if (grounded)
        {
            state = MovementState.walking;
            movespeed = walkspeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput+orientation.right * horizontalInput;

        //坂道
        if (OnSlope()&&!exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection()*movespeed*5f,ForceMode.Force);
        }
        //地上
        if(grounded)
            rb.AddForce(moveDirection.normalized*movespeed*10f,ForceMode.Force);
        //空中
        else if(!grounded)
            rb.AddForce(moveDirection.normalized*movespeed*2f*airMultiplier,ForceMode.Force);
    }

    void Jump()
    {
        exitingSlope = true;
        
        rb.velocity=new Vector3(rb.velocity.x,0f,rb.velocity.z);
        
        rb.AddForce(transform.up*jumpForce,ForceMode.Impulse);
    }
    void ResetJump()
    {
        readyToJump = true;
        
        exitingSlope = false;
    }
    
    private bool enableMovementOnNextTouch;
    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;

        cam.DoFov(grappleFov);
    }
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }
    
    public void ResetRestrictions()
    {
        activeGrapple = false;
        cam.DoFov(70f);
    }

    void SpeedControl()
    {
        Vector3 flatVel =new Vector3(rb.velocity.x,0f,rb.velocity.z);

        if (flatVel.magnitude > movespeed)
        {
            Vector3 limitedVel =flatVel.normalized*movespeed;
            rb.velocity = new Vector3(limitedVel.x,rb.velocity.y,limitedVel.z);
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            flatVel = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(flatVel.x, rb.velocity.y, flatVel.z);
        }
    }
    
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        
        return false;
    }
    
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
                                               + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection,slopeHit.normal).normalized;
    }
}
