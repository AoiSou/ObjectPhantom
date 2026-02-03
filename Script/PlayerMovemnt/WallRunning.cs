using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("壁走り")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimdSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("detection")]
    public float wallCheckDistance;
    public float minJunpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    
    [Header("Exiting")]
    private bool exitingWall;
    public float exitingWallTime;
    private float exitingWallTimer;
    
    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMove pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMove>();
    }
    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJunpHeight, whatIsGround);
    }
    
    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround()&&!exitingWall)
        {
            if(!pm.wallrunning)
                StartWallRun();

            
            if(Input.GetKeyDown(jumpkey))WallJump();
            
            if(wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitingWallTimer=exitingWallTime;
            }
            //壁ジャンプ
            if (Input.GetKeyDown(jumpkey))WallJump();
            
        }
        else if(exitingWall)
        {
            if(pm.wallrunning)
                StopWallRun();
            
            if(exitingWallTimer>0)
                exitingWallTimer -= Time.deltaTime;
            
            if(exitingWallTimer <= 0)
                exitingWall = false;
        }
        else
        {
            if(pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        
        wallRunTimer=maxWallRunTime;
        
        rb.velocity=new Vector3(rb.velocity.x,0f,rb.velocity.z);
        
        cam.DoFov(90f);
        if(wallLeft)cam.DoTile(-5f);
        if(wallRight)cam.DoTile(5f);
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;
        
        
        Vector3 WallNormal=wallRight?rightWallHit.normal:leftWallHit.normal;
        
        Vector3 WallFoward=Vector3.Cross(WallNormal,transform.up);

        if ((orientation.forward - WallFoward).magnitude > (orientation.forward - -WallFoward).magnitude)
            WallFoward = -WallFoward;
        
        rb.AddForce(WallFoward*wallRunForce,ForceMode.Force);
        
        if(upwardsRunning)
            rb.velocity=new Vector3(rb.velocity.x,wallClimdSpeed,rb.velocity.z);
        if(downwardsRunning)
            rb.velocity=new Vector3(rb.velocity.x,-wallClimdSpeed,rb.velocity.z);
        
        if(!(wallLeft&&horizontalInput>0)&&!(wallRight&&horizontalInput<0))
            rb.AddForce(-WallNormal*100,ForceMode.Force);
        
        if(useGravity)
            rb.AddForce(transform.up*gravityCounterForce,ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        
        cam.DoFov(70f);
        cam.DoTile(0f);
    }

    private void WallJump()
    {
        exitingWall = true;
        exitingWallTimer = exitingWallTime;
        
        Vector3 wallNormal=wallRight?rightWallHit.normal:leftWallHit.normal;
        
        Vector3 forceToApply =transform.up*wallJumpUpForce+wallNormal*wallJumpSideForce;
        
        rb.velocity=new Vector3(rb.velocity.x,0f,rb.velocity.z);
        rb.AddForce(forceToApply,ForceMode.Impulse);
        
        
    }
}
