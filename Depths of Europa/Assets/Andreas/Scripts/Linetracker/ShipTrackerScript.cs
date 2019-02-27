using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTrackerScript : MonoBehaviour
{

    [SerializeField, Range(0.1f, 2)] float _pollingIntervall = 0.5f;
    [SerializeField, Range(0.25f, 5)] float _distanceThreashold = 1;
    [SerializeField, Range(1, 10)] float _maxLineWidth;

    float _pollingCounter, _lastTime = 0;
    Vector2 _lastPosition;
    // Use this for initialization
    void Start()
    {
        _lastTime = Time.time;
        _lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        _pollingCounter += Time.deltaTime;
        if (_pollingCounter >= _pollingIntervall)
        {
            _pollingCounter = 0;
            float currentDistance = Vector2.Distance(transform.position, _lastPosition);
            if (currentDistance <= _distanceThreashold)
            {
                float timeThiccness = Time.time - _lastTime / currentDistance;
                if (timeThiccness >= _maxLineWidth)
                {

                }


            }

        }
    }
}