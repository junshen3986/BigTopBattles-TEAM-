using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingEachOther : MonoBehaviour
{

    public GameObject player;
    Vector3 playerPosition = new Vector3();

    public GameObject enemy; 
    Vector3 enemyPosition = new Vector3();

    public Vector3 distance;
    public GameObject returnGameobject;

    public GameObject[] players;
    public bool player2check;
    public bool didPlayer2check;
   
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        players = GameObject.FindGameObjectsWithTag("Player");
       
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Length > 1 && !didPlayer2check)
        {
            player2check = true;
        }
        if (!didPlayer2check && player2check)
        {
            enemy = FindPlayer2();
            didPlayer2check = true;
        } else if (didPlayer2check && player2check)
        {
            playerPosition = player.transform.position;
            enemyPosition = enemy.transform.position;
            distance = playerPosition - enemyPosition;

            if (distance.x < 0)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (distance.x > 0)
            {
                this.transform.localScale = new Vector3(-1, 1, 1);
            }

        }


    }
    GameObject FindPlayer2()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != this.gameObject)
            {
                returnGameobject = players[i];
            }
        }
        return returnGameobject;


    }
}
