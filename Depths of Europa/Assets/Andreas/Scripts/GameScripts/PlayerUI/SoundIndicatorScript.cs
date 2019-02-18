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
    [SerializeField, Range(1, 200), Tooltip("The ammount of line point to be updates in 1 frame")] int _lineUpdateAmmount = 10;
    [SerializeField, Tooltip("Should update fraction or update ammount be used, set true for ammount")] bool isUpdateAmmount = true;
    [SerializeField] AnimationCurve SoundCurve;
    Camera SoundIndicatorCapture;

    float _lineWidth;

    bool _firstLineRenderPass = true;

    [SerializeField, Range(0,1)] float _noise;

    readonly float SIN_BOUNDS = 1;

    Vector3[] _lineData;

    int _oldLineCount, _linePointUpdateCounter, _lineUpdateTarget, _shortLinePointUpdateCounter, _shortLineUpdateTarget;
    private IEnumerator _lineUpdate;
    float _yCenterInterpolator = 0, _traversal, _oldUpdateFrequency;


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        SoundIndicatorCapture = GetComponentInChildren<Camera>();
        _lineRenderer.positionCount = _numberOfLineSegments;
        _oldLineCount = _numberOfLineSegments;
        _lineWidth = _lineRenderer.widthMultiplier;
        _lineRenderer.widthMultiplier = 0;
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



            _yCenterInterpolator += 10 * curveFraction;

            Mathf.PingPong(_yCenterInterpolator, 0.5f);

            _traversal += _traversalSpeed * Time.deltaTime;


            float xPositionBase = transform.position.x + _indicatorBounds.x;

            float xPointPosition = Mathf.Lerp(xPositionBase - _indicatorBounds.width, xPositionBase + _indicatorBounds.width, curveProgression);

            float yPositionBase = transform.position.y + _indicatorBounds.y;


            float yPointCalc = ( Mathf.Sin((_frequencyModifier * Mathf.PI) + (curveProgression * _traversal * Mathf.PI * Mathf.Sin(_yCenterInterpolator * _debugLineModifier * Mathf.PI) * Mathf.Cos(_traversal * _debugLineModifier * Mathf.PI))));

            float normalizedYPoint = Mathf.InverseLerp(-SIN_BOUNDS, SIN_BOUNDS, yPointCalc);

            _lineData[i] = new Vector3(xPointPosition, yPositionBase, normalizedYPoint);
        }
    }


    private IEnumerator LineRender()
    {
        float volumeEvaluate = SoundCurve.Evaluate(_noise);
        while (true)
        {
            GenerateLineData();
            if (!isUpdateAmmount)
            {
                _lineUpdateTarget = Mathf.FloorToInt(_numberOfLineSegments * _lineUpdateFraction);
            }
            else if(isUpdateAmmount)
            {
                _lineUpdateTarget = _lineUpdateAmmount;
            }
            _linePointUpdateCounter = 0;
            volumeEvaluate = SoundCurve.Evaluate(_noise);
            float curveFraction = (float)1 / _numberOfLineSegments;
            for (int i = 0; i < _numberOfLineSegments; i++)
            {
                _linePointUpdateCounter++;

                float curveProgression = (float)i / _numberOfLineSegments;


                _yCenterInterpolator += 1 * curveFraction;

                Mathf.PingPong(_yCenterInterpolator, 0.5f);

                _traversal += _traversalSpeed * Time.deltaTime;

                float normalYPoint = _lineData[i].z;

                float YPosCalc = Mathf.Lerp(_lineData[i].y - (_indicatorBounds.height * volumeEvaluate), _lineData[i].y + (_indicatorBounds.height * volumeEvaluate), normalYPoint);

                _lineRenderer.SetPosition(i, new Vector2(_lineData[i].x,YPosCalc)); // Add sound mod
                if (_linePointUpdateCounter >= _lineUpdateTarget || _lineUpdateTarget < 1)
                {
                    yield return new WaitForEndOfFrame();
                    if (!isUpdateAmmount)
                    {
                        _lineUpdateTarget = Mathf.FloorToInt(_numberOfLineSegments * _lineUpdateFraction);
                    }
                    else if (isUpdateAmmount)
                    {
                        _lineUpdateTarget = _lineUpdateAmmount;
                    }
                    _linePointUpdateCounter = 0;
                    volumeEvaluate = SoundCurve.Evaluate(_noise);
                }
            }
            if(_firstLineRenderPass)
            {
                _firstLineRenderPass = false;
                _lineRenderer.widthMultiplier = _lineWidth;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
