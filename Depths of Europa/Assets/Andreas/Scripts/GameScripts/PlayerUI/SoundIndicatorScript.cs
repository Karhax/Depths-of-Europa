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
    [SerializeField, Range(0.05f,1),Tooltip("The percent of line point to be updates each frame (minumum of one)")] float _lineUpdateFraction = 0.1f;


    int _oldLineCount, _linePointUpdateCounter, _lineUpdateTarget;
    private IEnumerator _lineUpdate;
    float _yCenterInterpolator = 0, _traversal;


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rectTransform = GetComponent<RectTransform>();
        _lineRenderer.positionCount = _numberOfLineSegments;
        _oldLineCount = _numberOfLineSegments;
        _lineUpdate = LineRender();
    }

    private void OnValidate()
    {
        if(_numberOfLineSegments != _oldLineCount && _lineRenderer != null)
        {
            _lineRenderer.positionCount = _numberOfLineSegments;
            _oldLineCount = _numberOfLineSegments;
        }
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(_lineUpdate);
	}
	
	// Update is called once per frame
	void Update () {

	}

    private IEnumerator LineRender()
    {
        float yMax = 0;
        while (true)
        {
            _lineUpdateTarget = Mathf.FloorToInt(_numberOfLineSegments * _lineUpdateFraction);
            _linePointUpdateCounter = 0;
            float curveFraction = (float)1 / _numberOfLineSegments;
            for (int i = 0; i < _numberOfLineSegments; i++)
            {
                _linePointUpdateCounter++;

                float curveProgression = (float)i / _numberOfLineSegments;

                _traversal += _traversalSpeed * Time.deltaTime;


                _yCenterInterpolator += 1 * curveFraction;

                Mathf.PingPong(_yCenterInterpolator, 0.5f);

                float xPositionBase = _rectTransform.position.x + _indicatorBounds.x;

                float xPointPosition = Mathf.Lerp(xPositionBase - _indicatorBounds.width, xPositionBase + _indicatorBounds.width, curveProgression);

                float yPositionBase = _rectTransform.position.y + _indicatorBounds.y;

                float yPointCalc = (Mathf.Sin(_yCenterInterpolator * _debugLineModifier * Mathf.PI) * _magnitudeModifier * Mathf.Sin((_frequencyModifier * Mathf.PI) + (_traversal * Mathf.PI)));

                if(Mathf.Abs(yPointCalc) > yMax)
                {
                    yMax = Mathf.Abs(yPointCalc);
                    Debug.Log(yMax);

                }


                float normalizedYPoint = Mathf.InverseLerp(yPositionBase - _indicatorBounds.height, yPositionBase + _indicatorBounds.height, yPointCalc);



                _lineRenderer.SetPosition(i, new Vector3(xPointPosition, yPositionBase + yPointCalc));
                if (_linePointUpdateCounter >= _lineUpdateTarget || _lineUpdateTarget < 1)
                {
                    yield return new WaitForFixedUpdate();
                    _lineUpdateTarget = Mathf.FloorToInt(_numberOfLineSegments * _lineUpdateFraction);
                    _linePointUpdateCounter = 0;

                }
            }
            //yield return new WaitForSeconds(0.05f);
        }
    }
}
