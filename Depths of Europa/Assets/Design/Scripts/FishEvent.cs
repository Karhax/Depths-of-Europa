using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEvent : MonoBehaviour {

    private GameObject player;
    public GameObject BigFish;
    private bool hasSpawnedBigFish; 
	void Start ()
    {
        hasSpawnedBigFish = false;
       player = FindObjectOfType<MoveShip>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject ==player && !hasSpawnedBigFish)
        {
            hasSpawnedBigFish = true; ;
            BigFish = Instantiate(BigFish, transform.position + new Vector3(0,-10), Quaternion.identity) as GameObject;

        }
    }
}
