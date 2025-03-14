using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    Rigidbody enemyRB;

    PlayerMovement playerMovement;

    FacingEachOther facing;

    Vector3 attackForce;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        facing = GetComponentInParent<FacingEachOther>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (facing.distance.x < 0)
        {
            attackForce = new Vector3(5, 7, 0);
        }
        else if (facing.distance.x > 0)
        {
            attackForce = new Vector3(-5, 7, 0);
        }


        if (playerMovement.isReturning == true)
        {
            Debug.Log("ouch");
            enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            enemyRB.AddForce(attackForce, ForceMode.Impulse);
        }
    }

    
}
