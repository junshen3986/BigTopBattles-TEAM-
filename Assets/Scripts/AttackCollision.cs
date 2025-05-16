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

    GameManager gm;

    public HP enemyHP;

    public float attackPoint;
    private bool isAttacked;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        facing = GetComponentInParent<FacingEachOther>();
    }

    private void Update()
    {
        attackPoint = playerMovement.attackPointPM;
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


        if (!isAttacked)
        {
            Debug.Log("ouch");
            enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            enemyRB.AddForce(attackForce, ForceMode.Impulse);

            

            enemyHP = collision.gameObject.GetComponent<HP>();
            enemyHP.myHP -= attackPoint;
            print(enemyHP.myHP);
            isAttacked = true;
        } else if (playerMovement.isReturning == false)
        {
            isAttacked = false;
        }
    }

    
}
