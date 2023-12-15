using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJump : MonoBehaviour
{
    [Range(0f,0.2f)] public float extraScanDistance = 0.1f;
    public Text velX;
    public Text velY;
    public Text velZ;
    void Debugs()
    {
        //Debug.Log("vel.x = " + rb.velocity.x);
        velX.text = "vel.x = " + rb.velocity.x;
        //Debug.Log("vel.y = " + rb.velocity.y);
        velY.text = "vel.y = " + rb.velocity.y;
        //Debug.Log("vel.z = " + rb.velocity.z);
        velZ.text = "vel.z = " + rb.velocity.z;
    }

    [Header("Components")]
    public Rigidbody rb;

    [Header("Jump")]
    public float jumpForce = 10;
    public int extraJumps = 1;
    public int leftExtraJumps;

    [Header("Falling")]
    public float maxFallSpeed = 10;
    public float downGravityforce = 1;

    [Header("Jump Buffer")]
    public float jumpTimer = 1f;
    public float jumpBufferCooldown;

    [Header("Cyote Time")]
    public float cyoteTimer = 1f;
    public float cyoteCoooldown;


    [Header("Ground Check")]
    public float playerHeight = 2;
    public LayerMask whatIsGround;
    public bool grounded;
    public bool isUnder;
    public float groundDrag;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    //-----------------------------------------------------------------------------------
    #region start/update/fixedupdate
    private void Start()
    {
        
    }
    private void Update()
    {
        GroundCheck();
        DoubleJump();
        JumpBuffer();
        CyoteTime();

        Inputs();
    }
    private void FixedUpdate()
    {
        Friction();
        FallControl();
        FallClamp();
        Debugs();
    }
    #endregion
    //-----------------------------------------------------------------------------------    

    #region functions
    /*
    private void f()
    {

    }
    */
    
    private void Inputs()
    {
        // get when jump key is pressed
        if (jumpBufferCooldown > 0)  //Input.GetKey(jumpKey)
        {
            if (cyoteCoooldown > 0 || leftExtraJumps > 0) // grounded is put temporarly until cyote time is fixed || grounded
            {
                Jump();
            }
        }
    }
    private void Jump()
    {
        //if player is in air then he has less jumps
        if (!grounded && cyoteCoooldown < 0)
            leftExtraJumps--;        
        //reset y velocity to 0
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //give jump force impulse
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //reset cyote time
        cyoteCoooldown = 0f;
        //Debug.Log("cyote rest 0");
        //reset jump buffer time 
        jumpBufferCooldown = 0f;

    }
    
    private void Friction()
    {
        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void DoubleJump()
    {
        if (grounded)
            leftExtraJumps = extraJumps;
    }
    private void FallControl()
    {
        if(!grounded)
        {
            // early fall + faster fall
            // if player is moving down or jump button released early, apply extra gravity
            if ((rb.velocity.y > 0 && !Input.GetKey(jumpKey)) || rb.velocity.y < 0) 
            {
                //apply extra gravity to fall faster
                rb.AddForce(downGravityforce * Vector3.down, ForceMode.VelocityChange);
            }
        }
    }
    private void GroundCheck()
    {
        //head check
        isUnder = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + extraScanDistance);
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround);
    }
    private void FallClamp()
    {
        //clamp falling speed
        if (rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed), rb.velocity.z);

        }
    }
    private void JumpBuffer()
    {
        //jump buffer : if player presses jump button before landing, jump as soon as grounded
        if (Input.GetKeyDown(jumpKey))// && !grounded)
        {
            //jumpBuffer = true;
            jumpBufferCooldown = jumpTimer;
        }
        else
        {
            jumpBufferCooldown -= Time.deltaTime;
        }
    }
    private void CyoteTime()
    {
        //cyote time 
        if (grounded)
        {
            cyoteCoooldown = cyoteTimer;
            //Invoke(nameof(ResetCyoteFull), 1f);
            //Debug.Log("cyote rest full");
        }
        //cyote time cooldown
        else 
        {
            cyoteCoooldown -= Time.deltaTime;
        }
    }
    void ResetCyoteFull()
    {
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == whatIsGround)
        {
            Debug.Log("grounded = true");
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == whatIsGround)
        {
            Debug.Log("grounded = false");
            grounded = false;
        }
    }
    */

    #endregion
    /*double information in scripts:
     * grounded and ground check
     * keybindings
     * buffer jump works weird
     * 
    */
    /*TO DO
     * add player movement state
     * coyote time still has a problem
     * 
     * double jump
     * early fall- done
     * jump buffering-done
     * clamp falling speed-done
     * //hold crouch to stay on ledge
     * 
    */
}
