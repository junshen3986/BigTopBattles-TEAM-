using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    private Vector3 playerPos;

    void Start()
    {
        playerController = GameObject.FindWithTag("PlayerController").GetComponent<PlayerController>();
        playerPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementManager();
    }

    void MovementManager()
    {
        transform.position = playerPos;

        if (playerController.input_xMovement < 0)
        {
            playerPos.x += -1f;
        }
        else if (playerController.input_xMovement > 0) 
        {
            playerPos.x += 1f;
        }
    }
}
