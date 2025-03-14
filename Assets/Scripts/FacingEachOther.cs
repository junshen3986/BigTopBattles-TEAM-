using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingEachOther : MonoBehaviour
{

    [SerializeField] GameObject player;
    Vector3 playerPosition = new Vector3();

    [SerializeField] GameObject enemy; 
    Vector3 enemyPosition = new Vector3();

    public Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = player.transform.position;
        enemyPosition = enemy.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        enemyPosition = enemy.transform.position;
        distance = playerPosition - enemyPosition;

        if (distance.x < 0)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (distance.x > 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        
    }
}
