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

    public Animator playerAnimator;
    public int animatorID;
    public int tripleCount = 0;

    public bool attacked;
    public bool isReturning;

    void Start()
    {
        playerController = GameObject.FindWithTag("PlayerController").GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerMove = new Vector3 (0.15f, 0f,0f);
        
        jump = new Vector3(0.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementManager();
        AnimatorControl();
        AttackManager();
    }

    void MovementManager()
    {
        MoveLeftRight();
        MoveJump();

    }

    void AttackManager()
    {
        switch (animatorID)
        {
            case 0: //idle
                playerAnimator.SetInteger("Attack_ID", 0);
                break;

            case 1: // light punch
                playerAnimator.SetInteger("Attack_ID", 1);
                break;
        }
    }

    void AnimatorControl()
    {
        if (playerController.input_Punch)
        {
            if (!attacked)
            {
                animatorID = 1;
                //RepeatAttack();
                attacked = true;         
            }
            else if (attacked)
            {
                animatorID = 0;
            }
        }
        else {
            animatorID = 0;
            attacked = false;
        }

    }

    void ReturningTrue()
    {
        isReturning = true;
    }
    void ReturningFalse()
    {
        isReturning = false;
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

    /*void RepeatAttack()
    {
            if (tripleCount < 2)
            {
                tripleCount++;
            }
            else if (tripleCount >= 2)
            {
                tripleCount = 0;
            }
            playerAnimator.SetInteger("Attack_TripleCount", tripleCount);
    }
    */
    

}
