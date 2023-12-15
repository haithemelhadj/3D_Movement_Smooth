using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Declarations
    [Header("Movement")]
    public float moveSpeed;

    public float airMultiplier;
    public float sprintSpeed;
    public float walkSpeed;
    public float crouchSpeed;

    public bool isSprinting;
    public bool isCrouching;

    [Header("Crouching")]
    public bool isUnder;
    public Transform headObject;

    
    [Header("Ground Check")]
    [Range(0f, 0.2f)] public float extraScanDistance = 0.05f;
    public float playerHeight = 2;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Components")]
    public Transform orientation;
    public CapsuleCollider playerCollider;
    public Material material;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    [Header("Keybinds")]
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.LeftControl;
    //public KeyCode jump = KeyCode.Space;



    [Header("Player State")]
    public playerstate playerState;
    public enum playerstate
    {
        walking,
        sprinting,
        crounching,
        inAir
    }

    /*
    public float maxSpeed;
    public float currentSpeed;
    public float acceleration;
    public float decelration;
    */
    #endregion


    //-----------------------------------------------------------------------------------
    #region start/update/fixedupdate
    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    private void Update()
    {
        MyInput();
        Debug.Log("speed 1 is " + moveSpeed);
        Crouching();
        Debug.Log("speed 2 is " + moveSpeed);
        Sprinting();
        Debug.Log("speed 3 is " + moveSpeed);
        speedControl();
        Debug.Log("speed 4 is " + moveSpeed);



        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround);
        //head check
        isUnder = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + extraScanDistance);

        /*
        if (grounded && Input.GetKeyDown(sprint) && verticalInput > 0 && ) 
        {
            //issprinting true
            playerState = playerstate.sprinting;
        }
        else if(grounded && Input.GetKeyDown(crouch))
        {
            //iscrouching true
            playerState = playerstate.crounching;
        }
        else if (playerState == playerstate.crounching && !isUnder)
        {

        }
        else
        {
            //iswalking true
            playerState = playerstate.walking;
        
        }
        */
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    #endregion
    //-----------------------------------------------------------------------------------
    #region debugs
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + extraScanDistance));//, Color.red
        Gizmos.DrawRay(headObject.position, Vector3.up * extraScanDistance);//, Color.red
    }
    #endregion

    #region Input

    // get player input
    private void MyInput()
    {
        //get x,z movement direction
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
    }
    private void Crouching()
    {
        if(Input.GetKeyDown(crouch) && grounded)
        {
            if(isCrouching && !isUnder)
            {
                material.color = Color.blue;
                moveSpeed = walkSpeed;
                isCrouching = false;
                //change collider size
                playerCollider.height = playerHeight;
                playerCollider.center = Vector3.zero;

            }
            else
            {
                material.color = Color.red;
                moveSpeed = crouchSpeed;
                isCrouching = true;
                isSprinting = false;
                //change collider size
                playerCollider.height = playerHeight * 0.5f;
                playerCollider.center = new Vector3(0f, -0.5f, 0f);
            }
        }
    }
    private void Sprinting()
    {
        if (Input.GetKeyDown(sprint))//toggle sprint
        {
            if (grounded && !isCrouching)
            {
                if(!isSprinting)//&& !isCrouching && grounded)//if is not running then run
                {
                    material.color = Color.green;
                    moveSpeed = sprintSpeed; 
                    isSprinting = true;
                }
                else//if is run then stop running
                {
                    material.color = Color.white;
                    moveSpeed = walkSpeed;
                    isSprinting = false;
                }   
            } 
        }        
    }
    #endregion


    #region Control PlayerState

    /*
    private void playerStateControl()
    {

        //set player playerState before moving
        if(!grounded)
        {
            playerState = playerstate.inAir;
        }
        else if(Input.GetKey(sprint))
        {
            playerState = playerstate.sprinting;
        }
        else if(Input.GetKey(crouch))
        {
            playerState = playerstate.crounching;
        }
        else
        {
            playerState = playerstate.walking;
        }
        //player playerState switch 
        switch(playerState)
        {
            case playerstate.walking:
                moveSpeed = walkSpeed;
                break;
            case playerstate.sprinting:
                moveSpeed = sprintSpeed;
                break;
            case playerstate.crounching:
                moveSpeed = crouchSpeed;
                break;
            case playerstate.inAir:
                moveSpeed = 10f;
                break;
        }        
    }
    */
    #endregion


    #region Movement
    //player movement logic
    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
              
    }

    //limit player speed to never go over what is set in moveSpeed
    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //limit  velocity if needed
        if(flatVel.magnitude>moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    #endregion

}
