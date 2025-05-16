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

    public float attackPointPM;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerMove = new Vector3 (0.15f, 0f,0f);
        
        jump = new Vector3(0.0f, 1.0f, 0.0f);
    }

    private void Update()
    {
        AnimatorControl();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        MovementManager();
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
                attackPointPM = 5;
            }

           
            }
        else if (playerController.input_HeavyPunch)
        {
            if (!attacked)
            {
                animatorID = 2;
                //RepeatAttack();
                attacked = true;
                attackPointPM = 10;
            }

        }



        
        else 
        {
            animatorID = 0;
            attackPointPM = 0;
            attacked = false;
        }

    }

    void SetAttackDamage()
    {

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
