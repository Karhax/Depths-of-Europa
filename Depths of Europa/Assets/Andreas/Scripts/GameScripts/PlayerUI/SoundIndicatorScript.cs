using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIndicatorScript : MonoBehaviour {
    private LineRenderer _lineRenderer;
    [SerializeField] Rect _indicatorBounds;
    RectTransform _rectTransform;
    [SerializeField, Range(10,200)] int _numberOfLineSegments = 100;
    [SerializeField, Range(0,1)] float _debugLineModifier;
    [SerializeField] float _frequencyModifier = 10, _magnitudeModifier = 10;

    float _yCenterInterpolator = 0;
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rectTransform = GetComponent<RectTransform>();
        _lineRenderer.positionCount = _numberOfLineSegments;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < _numberOfLineSegments; i++)
        {
            float curveProgression = (float)i / _numberOfLineSegments;
            _yCenterInterpolator = 2 * curveProgression;
            Mathf.PingPong(_yCenterInterpolator, 1);
            float xPositionBase = transform.position.x + _indicatorBounds.x;
            float xPointPosition = Mathf.Lerp(xPositionBase - _indicatorBounds.width, xPositionBase + _indicatorBounds.width,curveProgression);
            float yPositionBase = transform.position.y + _indicatorBounds.y;
            float yPointPosition = yPositionBase + _magnitudeModifier * Mathf.Sin(_frequencyModifier * _debugLineModifier * _yCenterInterpolator * Time.time);
            _lineRenderer.SetPosition(i, new Vector3(xPointPosition, yPointPosition));
        }
	}
}
