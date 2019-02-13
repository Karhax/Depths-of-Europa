using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIndicatorScript : MonoBehaviour {
    private LineRenderer _lineRenderer;
    [SerializeField] Rect _indicatorBounds;
    [SerializeField, Range(10,200)] int _numberOfLineSegments = 100;
    [SerializeField, Range(0,1)] float _debugLineModifier;
    [SerializeField] float _frequencyModifier = 10,  _traversalSpeed = 0.2f;
    [SerializeField, Range(0.05f,1),Tooltip("The percent of line point to be updates each frame (minumum of one)")] float _lineUpdateFraction = 0.1f;
    Camera SoundIndicatorCapture;

    readonly float SIN_BOUNDS = 1;

    Vector3[] _lineData;

    int _oldLineCount, _linePointUpdateCounter, _lineUpdateTarget;
    private IEnumerator _lineUpdate;
    float _yCenterInterpolator = 0, _traversal, _oldUpdateFrequency;


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        SoundIndicatorCapture = GetComponentInChildren<Camera>();
        _lineRenderer.positionCount = _numberOfLineSegments;
        _oldLineCount = _numberOfLineSegments;
        GenerateLineData();
        _lineUpdate = LineRender();
        _oldUpdateFrequency = _lineUpdateFraction;
    }

    private void OnValidate()
    {
        if (_lineUpdateFraction == _oldUpdateFrequency)
        {
            if (_numberOfLineSegments != _oldLineCount && _lineRenderer != null)
            {
                _lineRenderer.positionCount = _numberOfLineSegments;
                _oldLineCount = _numberOfLineSegments;
            }
            GenerateLineData();
        }
        else
        {
            _oldUpdateFrequency = _lineUpdateFraction;
        }
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(_lineUpdate);
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void GenerateLineData()
    {
        _lineData = new Vector3[_numberOfLineSegments];
        float curveFraction = (float)1 / _numberOfLineSegments;
        for (int i = 0; i < _numberOfLineSegments; i++)
        {
            float curveProgression = (float)i / _numberOfLineSegments;



            _yCenterInterpolator += 1 * curveFraction;

            Mathf.PingPong(_yCenterInterpolator, 0.5f);

            _traversal += _traversalSpeed * Time.deltaTime;


            float xPositionBase = transform.position.x + _indicatorBounds.x;

            float xPointPosition = Mathf.Lerp(xPositionBase - _indicatorBounds.width, xPositionBase + _indicatorBounds.width, curveProgression);

            float yPositionBase = transform.position.y + _indicatorBounds.y;

            float yPointCalc = (Mathf.Sin(_yCenterInterpolator * _debugLineModifier * Mathf.PI) * Mathf.Sin((_frequencyModifier * Mathf.PI) + (curveProgression * _traversal * Mathf.PI)));




            float normalizedYPoint = Mathf.InverseLerp(-SIN_BOUNDS, SIN_BOUNDS, yPointCalc);

            float realYPos = Mathf.Lerp(yPositionBase - _indicatorBounds.height, yPositionBase + _indicatorBounds.height, normalizedYPoint);

            _lineData[i] = new Vector3(xPointPosition, realYPos, normalizedYPoint);
        }
    }


    private IEnumerator LineRender()
    {
        while (true)
        {
            GenerateLineData();
            _lineUpdateTarget = Mathf.FloorToInt(_numberOfLineSegments * _lineUpdateFraction);
            _linePointUpdateCounter = 0;
            float curveFraction = (float)1 / _numberOfLineSegments;
            for (int i = 0; i < _numberOfLineSegments; i++)
            {
                _linePointUpdateCounter++;

                float curveProgression = (float)i / _numberOfLineSegments;



                _yCenterInterpolator += 1 * curveFraction;

                Mathf.PingPong(_yCenterInterpolator, 0.5f);

                _traversal += _traversalSpeed * Time.deltaTime;

                float normalYPoint = _lineData[i].z;

                _lineRenderer.SetPosition(i, new Vector3(_lineData[i].x, _lineData[i].y)); // Add sound mod
                if (_linePointUpdateCounter >= _lineUpdateTarget || _lineUpdateTarget < 1)
                {
                    yield return new WaitForEndOfFrame();
                    _lineUpdateTarget = Mathf.FloorToInt(_numberOfLineSegments * _lineUpdateFraction);
                    _linePointUpdateCounter = 0;

                }
            }
            //yield return new WaitForSeconds(0.05f);
        }
    }
}
