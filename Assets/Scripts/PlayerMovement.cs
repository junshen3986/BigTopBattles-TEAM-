using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController playerController;
    private Rigidbody playerRb;
    private Vector3 playerMove;

    public bool isGrounded;
    private Vector3 jump;
    public float jumpForce = 7f;

    public Animator playerAnimator;
    public int animatorID;
    public int tripleCount = 0;

    public bool attacked;
    public bool isReturning;

    public bool isCrouching;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
        MoveJump();
        MoveCrouch();
        MoveLeftRight();
        

    }

    void AttackManager()
    {
        switch (animatorID)
        {
            case 0: //idle
                playerAnimator.SetInteger("Attack_ID", 0);
                break;

            case 1: // light standing
                playerAnimator.SetInteger("Attack_ID", 1);
                break;
            case 2: // heavy standing
                playerAnimator.SetInteger("Attack_ID", 2);
                break;
            case 3: // light crouch
                playerAnimator.SetInteger("Attack_ID", 3);
                break;
            case 4: // heavy crouch
                playerAnimator.SetInteger("Attack_ID", 4);
                break;
            case 5: // light jump
                playerAnimator.SetInteger("Attack_ID", 5);
                break;
            case 6: // heavy jump
                playerAnimator.SetInteger("Attack_ID", 6);
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

        if (playerController.input_HeavyPunch)
        {
            if (!attacked)
            {
                animatorID = 2;
                //RepeatAttack();
                attacked = true;
            }
            else if (attacked)
            {
                animatorID = 0;
            }
        }
        else
        {
            animatorID = 0;
            attacked = false;
        }


        if (playerController.input_Kick)
        {
            if (!attacked)
            {
                animatorID = 3;
                //RepeatAttack();
                attacked = true;
            }
            else if (attacked)
            {
                animatorID = 0;
            }
        }
        else
        {
            animatorID = 0;
            attacked = false;
        }


        if (playerController.input_HeavyKick)
        {
            if (!attacked)
            {
                animatorID = 4;
                //RepeatAttack();
                attacked = true;
            }
            else if (attacked)
            {
                animatorID = 0;
            }
        }
        else
        {
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
        if (playerController.input_xMovement < 0 && !isCrouching)
        {
            playerAnimator.SetBool("isWalking", true);
            transform.position -= playerMove;
        }
        else if (playerController.input_xMovement > 0 && !isCrouching)
        {
            playerAnimator.SetBool("isWalking", true);
            transform.position += playerMove;  
        } else
        {
            playerAnimator.SetBool("isWalking", false);
        }
    }

    void MoveJump()
    {
       
            if (playerController.input_Jump && isGrounded )
        {
            playerAnimator.SetBool("isJumping", true);
            playerRb.AddForce(jump * jumpForce, ForceMode.Impulse);
        } else if (isGrounded)
        {
            playerAnimator.SetBool("isJumping", false);
        }
    }

    void MoveCrouch()
    {
        if (playerController.input_Crouch)
        {
            playerAnimator.SetBool("isCrouching", true);
            isCrouching = true;
        } else if (!playerController.input_Crouch)
        {
            playerAnimator.SetBool("isCrouching", false);
            isCrouching = false;
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
