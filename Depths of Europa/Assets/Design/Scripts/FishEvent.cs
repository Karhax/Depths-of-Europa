using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEvent : MonoBehaviour {

    
    [SerializeField, ] GameObject bigFish;
    

    GameObject player;
    bool hasSpawnedBigFish;
    void Start ()
    {
        hasSpawnedBigFish = false;
        player = FindObjectOfType<MoveShip>().gameObject;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player && !hasSpawnedBigFish)
        {
            hasSpawnedBigFish = true; 
            bigFish = Instantiate(bigFish, transform.position + new Vector3(0,-10), Quaternion.identity) as GameObject;

        }
    }
}
