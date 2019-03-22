using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class BigMonsterMovementEnable : MonoBehaviour
{
    [SerializeField] GameObject _bigFish;
    [SerializeField] bool _startOnTrigger = false;

    private void Awake()
    {
        if (_startOnTrigger)
            _bigFish.SetActive(false);
        else
            Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_OUTSIDE))
        {
            _bigFish.SetActive(true);
            Destroy(this);
        }
            
    }
}
