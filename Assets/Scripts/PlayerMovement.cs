using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody playerRb;
    private Vector3 playerMove;

    private bool isGrounded;
    private Vector3 jump;
    public float jumpForce = 7f;


    void Start()
    {
        playerController = GameObject.FindWithTag("PlayerController").GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        playerMove = new Vector3 (0.15f, 0f,0f);
        
        jump = new Vector3(0.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementManager();
    }

    void MovementManager()
    {
        MoveLeftRight();
        MoveJump();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stage"))
        {
            isGrounded = true;
        }      
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void MoveLeftRight()
    {
        if (playerController.input_xMovement < 0)
        {
            transform.position -= playerMove;
        }
        else if (playerController.input_xMovement > 0)
        {
            transform.position += playerMove;  
        }
    }

    void MoveJump()
    {
       
            if (playerController.input_Jump && isGrounded )
        {
            playerRb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
    }
}
