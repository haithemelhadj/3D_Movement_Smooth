using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("Refrences")]
    public PlayerManager playerManager;
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;




        bool locked = true;
    private void Update()
    {
        //lock cursor 
        if (Input.GetKey(KeyCode.L)) locked = !locked;
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }else
        {
            Cursor.lockState= CursorLockMode.None;
            Cursor.visible = true;
        }


        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        //if player is hanging he can't rotate
        if (playerManager.isHanging)
            return;
        //rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(inputDir!=Vector3.zero)
        {
            playerObj.forward=Vector3.Slerp(playerObj.forward,inputDir.normalized,Time.deltaTime*rotationSpeed); 

        }
    }

}
