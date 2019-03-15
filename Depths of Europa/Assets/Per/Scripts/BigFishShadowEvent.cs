using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFishShadowEvent : MonoBehaviour {

    [SerializeField] private Sprite _bigFishSprite;
    [SerializeField] private GameObject _bigFishPrefab;
    [SerializeField] [Range(0.1f, 10)] private float _moveSpeed = 2;
    [SerializeField] [Range(180, -180)] private float _startingAngle = 0;
    [SerializeField] [Range(-10, 10)] private float _turnSpeed = 0;
    [SerializeField] [Range(-64, 64)] private float _offsetX = 0;
    [SerializeField] [Range(-64, 64)] private float _offsetY = 0;

    private GameObject _bigFishShadow = null;
    private BigFishRemovalDetection _detectorScript = null;

    private void Update()
    {
        // If a BigFish exists, it should move according to the settings
        if (_bigFishShadow != null)
        {
            _bigFishShadow.transform.Rotate(Vector3.forward, _turnSpeed * Time.deltaTime * -1);

            float angle = _bigFishShadow.transform.localEulerAngles.z;
            float x = Mathf.Sin(angle * Mathf.PI / 180) * _moveSpeed * -1;
            float y = Mathf.Cos(angle * Mathf.PI / 180) * _moveSpeed;
            Vector3 movePerFrame = new Vector3(x, y, 0);

            _bigFishShadow.transform.localPosition += movePerFrame * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Event is only triggered by the player, and it can only create one BigFish
        if (other.gameObject.CompareTag(Statics.Tags.PLAYER_OUTSIDE) && _bigFishShadow == null)
        {
            Vector3 location = new Vector3(other.transform.position.x + _offsetX, other.transform.position.y + _offsetY, this.transform.position.z);
            _bigFishShadow = Instantiate(_bigFishPrefab, location, Quaternion.Euler(0, 0, _startingAngle));

            _detectorScript = _bigFishShadow.GetComponent<BigFishRemovalDetection>();
            _detectorScript.ActiveAreaExit += RemoveEvent;
        }
    }

    private void RemoveEvent()
    {
        // Removes the BigFish and this object
        _detectorScript.ActiveAreaExit -= RemoveEvent;
        Destroy(_bigFishShadow);
        Destroy(this.gameObject);
    }
}
