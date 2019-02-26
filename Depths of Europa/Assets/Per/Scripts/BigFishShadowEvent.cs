using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFishShadowEvent : MonoBehaviour {

    [SerializeField] private Transform _bigFishShadow;
    [SerializeField] [Range(0.1f, 10)] private float _moveSpeed = 2;
    [SerializeField] [Range(-5, 5)] private float _turnSpeed = 0;
    [SerializeField] [Range(1, 100)] private float _travelTimeDuration = 50;
    [SerializeField] [Range(-64, 64)] private float _offsetX = 0;
    [SerializeField] [Range(-64, 64)] private float _offsetY = 0;
    
    private bool _moving = false;
    private Timer _travelTimer;

    private void Awake()
    {
        _travelTimer = new Timer(_travelTimeDuration);
    }

    private void Update()
    {
        if (_moving)
        {
            _bigFishShadow.Rotate(Vector3.forward, _turnSpeed * Time.deltaTime * -1);

            float angle = _bigFishShadow.localEulerAngles.z;
            float x = Mathf.Sin(angle * Mathf.PI / 180) * _moveSpeed * -1;
            float y = Mathf.Cos(angle * Mathf.PI / 180) * _moveSpeed;
            Vector3 movePerFrame = new Vector3(x, y, 0);

            _bigFishShadow.localPosition += movePerFrame * Time.deltaTime;
            
            _travelTimer.Time += Time.deltaTime;
            if (_travelTimer.Expired())
            {
                Destroy(this.transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Statics.Tags.PLAYER_OUTSIDE))
        {
            _moving = true;
            Vector3 location = new Vector3(other.transform.position.x + _offsetX, other.transform.position.y + _offsetY, _bigFishShadow.transform.position.z);
            _bigFishShadow.transform.position = location;
        }
    }
}
