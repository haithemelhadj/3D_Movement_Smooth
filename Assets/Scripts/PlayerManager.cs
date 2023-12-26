using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    //public PlayerManager playerManager;
    public Rigidbody rb;
    public Animator animator;

    [Header("Bools")]
    public bool isHanging;
    public bool grounded;


    [Header("Keybinds")]
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    


    [Header("Ground Check")]
    [Range(0f, 0.2f)] public float extraScanDistance = 0.05f;
    public float playerHeight = 2;
    public LayerMask whatIsGround;



    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround);
    }
}
