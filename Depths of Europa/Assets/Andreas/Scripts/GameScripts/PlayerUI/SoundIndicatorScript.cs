using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIndicatorScript : MonoBehaviour {
    private LineRenderer _lineRenderer;
    [SerializeField] Rect _indicatorBounds;
    RectTransform _rectTransform;
    [SerializeField, Range(10,200)] int _numberOfLineSegments = 100;
    [SerializeField, Range(0,1)] float _debugLineModifier;
    [SerializeField] float _frequencyModifier = 10, _magnitudeModifier = 10, _traversalSpeed = 0.2f;

    float _yCenterInterpolator = 0, _traversal;
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
        float curveFraction = (float)1 / _numberOfLineSegments;
        for (int i = 0; i <= _numberOfLineSegments; i++)
        {
            float curveProgression = (float)i / _numberOfLineSegments;

            _traversal += _traversalSpeed * Time.deltaTime;

            _yCenterInterpolator += 1 * curveFraction;

            Mathf.PingPong(_yCenterInterpolator, 0.5f);

            float xPositionBase = transform.position.x + _indicatorBounds.x;

            float xPointPosition = Mathf.Lerp(xPositionBase - _indicatorBounds.width, xPositionBase + _indicatorBounds.width,curveProgression);

            float yPositionBase = transform.position.y + _indicatorBounds.y;

            float yPointPosition = yPositionBase + (Mathf.Sin(_yCenterInterpolator * _debugLineModifier * Mathf.PI) * _magnitudeModifier * Mathf.Sin(_frequencyModifier * Mathf.PI));

            _lineRenderer.SetPosition(i, new Vector3(xPointPosition, yPointPosition));
        }
	}
}
