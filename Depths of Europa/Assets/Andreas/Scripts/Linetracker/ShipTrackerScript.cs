using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTrackerScript : MonoBehaviour {

    [SerializeField, Range(0.1f, 2)] float _pollingIntervall = 0.5f;
    [SerializeField, Range(0.25f, 5)] float _distanceThreashold = 1;

    float _pollingCounter, _lastTime = 0;
	// Use this for initialization
	void Start () {
        _lastTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        _pollingCounter += Time.deltaTime;
        if(_pollingCounter >= _pollingIntervall)
        {
            _pollingCounter = 0;

        }

	}
}
