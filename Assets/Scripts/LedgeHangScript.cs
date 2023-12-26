using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeHangScript : MonoBehaviour
{

    public bool isFalling;
    public Transform overHead;
    public Transform underHead;

    public Rigidbody rb;
    public Transform orientation;
    bool hanging;

    
    void LedgeGrab()
    {
        //check if the player is falling and is not hanging
        if (rb.velocity.y < 0 && !hanging)
        {
            //cast a ray down
            RaycastHit downHit;
            Vector3 LineDownStart = (transform.position + Vector3.up * 1.5f) + transform.forward;
            Vector3 LineDownEnd =  (transform.position + Vector3.up * 0.7f)+transform.forward;
            Physics.Linecast(LineDownStart, LineDownEnd, out downHit, LayerMask.GetMask("Ground"));
            Debug.DrawLine(LineDownStart, LineDownEnd);
            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Vector3 LineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;
                Physics.Linecast(lineFwdStart, LineFwdEnd, out fwdHit, LayerMask.GetMask("Ground"));
                Debug.DrawLine(lineFwdStart, LineFwdEnd);
                if (fwdHit.collider != null)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    hanging = true;
                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwdHit.normal;
                }
                //bugs in this code:
                //no gravity reset
            }
        }
    }

    public void LedgeDetection()
    {

        /*
        if(rb.velocity.y < 0)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;            
        }
        */

        /*
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extraScanDistance, whatIsGround);

        //Ray overHeadHit =  ;//Physics.Raycast(overHead.position, Vector3.forward, 1f);
        //bool underHeadHit = Physics.Raycast(underHead.position, Vector3.forward, 1f);

        Ray overHeadHit = new Ray(overHead.position, orientation.forward); 
        //Physics.Raycast(overHeadHit, out RaycastHit overHit);
        Ray underHeadHit = new Ray(overHead.position, orientation.forward);
        Debug.DrawRay(overHead.position, orientation.forward, Color.red, 1f);
        Debug.DrawRay(underHead.position, orientation.forward, Color.red, 1f);
        //if(underHead && !overHead && isFalling)
        if (Physics.Raycast(underHeadHit, out RaycastHit underHit, 1f) && Physics.Raycast(overHeadHit, out RaycastHit overHit, 1f) && isFalling) 
        {
            Debug.Log("is hanging");
            Vector3 ledgePos = Vector3.zero; // is the x horizontal of the player, the forward of the player when hitting the ledge, and the y is the hight of the ledge
        //Physics.Raycast(underHeadHit, out RaycastHit underHit);

            ledgePos.z = underHit.point.z;// this is the z
            Ray hightCheck = new Ray(new Vector3(underHit.point.x,overHead.position.y,underHit.point.z), Vector3.down);
            Physics.Raycast(underHeadHit, out RaycastHit hightCheckHit);
        Debug.DrawRay(new Vector3(underHit.point.x, overHead.position.y, underHit.point.z), Vector3.down, Color.blue, 1f);
            ledgePos.y = hightCheckHit.point.y;
            ledgePos.x = underHit.point.x;
            rb.constraints = RigidbodyConstraints.FreezeAll;

        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        LedgeGrab();
        //LedgeDetection();
    }

    /*
     * to hang on ledge :
     * when in air cast two rays :
     * one that is above the player head 
     * one that is below the player face 
     * if the the above ray is hitting nothing and the below ray is hitting something
     * then set the player to ledge hanging state
     * 
    */
}
