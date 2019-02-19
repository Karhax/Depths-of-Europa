﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIndicatorScript : MonoBehaviour {

    #region Serialized Variables

    [SerializeField] Rect _indicatorBounds;
    [SerializeField, Range(10,200)] int _numberOfLineSegments = 100;
    [SerializeField, Range(0,1)] float _SineWaveLineModifier;
    [SerializeField] float _frequencyModifier = 10,  _traversalSpeed = 0.2f;
    [SerializeField, Range(1, 200), Tooltip("The ammount of line point to be updates in 1 frame")] int _lineUpdateAmmount = 10;
    [SerializeField, Tooltip("Evaluates the curve at a position based on the noise value (on the x-axis) and returns the value from the y-axis(plase ensure that the value is between 0 and 1))")] AnimationCurve _soundCurve;
    [SerializeField] Gradient _volumeColourGradient, _darknessAndAlphaGradient;

    #endregion

    #region private variables

    //Camera _soundIndicatorCapture;

    private LineRenderer _lineRenderer;

    float _lineWidth;

    bool _firstLineRenderPass = true;

    //int _consistentIndex, _currentWriteKey = 0;

    Vector4[] _lineData;

    int _oldLineCount, _linePointUpdateCounter, _lineUpdateTarget, _shortLinePointUpdateCounter, _shortLineUpdateTarget;

    private IEnumerator _lineUpdate;

    float _yCenterInterpolator = 0, _traversal, _oldUpdateFrequency, _lineUpdateFraction, _lastColourIndex, _lastColourIndexLoudestVolume;

    Gradient _internalColourGradient = new Gradient();

    GradientColorKey[] _internalColourKeys;

    GradientAlphaKey[] _internalAlphaKeys;

    bool _gradientFirstGeneratePass = true;

    //float _colourUpdateFraction;


    #endregion

    #region Read Only Variables

    readonly float SIN_BOUNDS = 1;

    readonly int MAX_GRADIENT_KEYS = 8, MAX_GRADIENT_INDEX = 7;

    #endregion

    #region Debug Variables

    [SerializeField, Range(0,1)] float _noise;


    #endregion



    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        //_soundIndicatorCapture = GetComponentInChildren<Camera>();
        _lineRenderer.positionCount = _numberOfLineSegments;
        _oldLineCount = _numberOfLineSegments;
        _lineWidth = _lineRenderer.widthMultiplier;
        _lineRenderer.widthMultiplier = 0;
        GenerateLineData();
        _lineUpdate = LineRender();
        //_colourUpdateFraction = Mathf.InverseLerp(0, MAX_GRADIENT_INDEX, 1);
    }

    private void OnValidate()
    {
        if (_numberOfLineSegments != _oldLineCount && _lineRenderer != null)
        {
            _lineRenderer.positionCount = _numberOfLineSegments;
            _oldLineCount = _numberOfLineSegments;
        }
        GenerateLineData();
        _lineUpdateFraction = _lineUpdateAmmount / _numberOfLineSegments;
    }

    void Start () {
        StartCoroutine(_lineUpdate);
	}
	
    private void GenerateLineData()
    {

        _lineData = new Vector4[_numberOfLineSegments];
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


            float yPointCalc = ( Mathf.Sin((_frequencyModifier * Mathf.PI) + (curveProgression * _traversal * Mathf.PI * Mathf.Sin(_yCenterInterpolator * _SineWaveLineModifier * Mathf.PI) * Mathf.Cos(_traversal * _SineWaveLineModifier * Mathf.PI))));

            float normalizedYPoint = Mathf.InverseLerp(-SIN_BOUNDS, SIN_BOUNDS, yPointCalc);

            _lineData[i] = new Vector4(xPointPosition, yPositionBase, normalizedYPoint, curveProgression);
        }
    }

    private void GenerateColourProfile(float volume, float drawHeadPos)
    {
        float assignedPos;
        
        if(_gradientFirstGeneratePass)
        {
            _gradientFirstGeneratePass = false;
            GradientSetUp();
        }
        
        int headerIndex = Mathf.FloorToInt(MAX_GRADIENT_KEYS * drawHeadPos);
        if(!(headerIndex == _lastColourIndex && _lastColourIndexLoudestVolume >= volume))
        {
            _lastColourIndex = headerIndex;
            _lastColourIndexLoudestVolume = volume;
            if (headerIndex == MAX_GRADIENT_KEYS)
            {
                headerIndex = MAX_GRADIENT_INDEX;
            }
            if (drawHeadPos <= 0.75f * _lineUpdateFraction)
            {
                assignedPos = 0;
            }
            else if (drawHeadPos >= 1 - (0.75f * _lineUpdateFraction))
            {
                assignedPos = 1;
            }
            else
            {
                assignedPos = drawHeadPos;
            }
            //Debug.Log("HeadePos: " + drawHeadPos + " \nHeader Index: " + headerIndex);
            SetGradientTime(headerIndex, assignedPos);
            _internalColourKeys[headerIndex].color = GetVolymeColour(volume);
        }

        for (int i = 0; i < MAX_GRADIENT_KEYS; i++)
        {


                

            
            
        }
        //Debug.Log(_internalColourKeys[0].time);

        //_currentWriteKey = _currentWriteKey.AddAndRepeatInt(MAX_GRADIENT_INDEX);



        _internalColourGradient.SetKeys(_internalColourKeys, _internalAlphaKeys);
        _lineRenderer.colorGradient = _internalColourGradient;
    }

    private void SetGradientTime(int i,float time)
    {
        _internalColourKeys[i].time = time;
        _internalAlphaKeys[i].time = time;
    }

    private void GradientSetUp()
    {
        _internalColourKeys = new GradientColorKey[MAX_GRADIENT_KEYS];
        _internalAlphaKeys = new GradientAlphaKey[MAX_GRADIENT_KEYS];
        for(int i = 0; i < MAX_GRADIENT_KEYS; i++)
        {
            _internalColourKeys[i].time = Mathf.InverseLerp(0, MAX_GRADIENT_INDEX, i);
            _internalAlphaKeys[i].time = Mathf.InverseLerp(0, MAX_GRADIENT_INDEX, i);
            _internalAlphaKeys[i].alpha = 255;
        }
    }

    private Color GetVolymeColour(float volume)
    {
        return _volumeColourGradient.Evaluate(volume);
    }

    private void GenerateBrightnessKey()
    {

    }

    private IEnumerator LineRender()
    {
        float volumeEvaluate;
        volumeEvaluate = Mathf.Clamp01(_soundCurve.Evaluate(_noise));
        float curveFraction = (float)1 / _numberOfLineSegments;
        GenerateColourProfile(volumeEvaluate, 0);
        while (true)
        {
            GenerateLineData();
            _lineUpdateTarget = _lineUpdateAmmount;
            _linePointUpdateCounter = 0;
            volumeEvaluate = Mathf.Clamp01(_soundCurve.Evaluate(_noise));
            curveFraction = (float)1 / _numberOfLineSegments;

            for (int i = 0; i < _numberOfLineSegments; i++)
            {
                _linePointUpdateCounter++;
                //float curveProgression = (float)i / _numberOfLineSegments;


                _yCenterInterpolator += 1 * curveFraction;

                Mathf.PingPong(_yCenterInterpolator, 0.5f);

                _traversal += _traversalSpeed * Time.deltaTime;

                float normalYPoint = _lineData[i].z;

                float YPosCalc = Mathf.Lerp(_lineData[i].y - (_indicatorBounds.height * volumeEvaluate), _lineData[i].y + (_indicatorBounds.height * volumeEvaluate), normalYPoint);

                _lineRenderer.SetPosition(i, new Vector2(_lineData[i].x,YPosCalc));

                if (_linePointUpdateCounter >= _lineUpdateTarget)
                {
                    //_noise = Random.value;
                    volumeEvaluate = Mathf.Clamp01(_soundCurve.Evaluate(_noise));

                    GenerateColourProfile(volumeEvaluate, _lineData[i].w);

                    yield return new WaitForEndOfFrame();

                    _lineUpdateTarget = _lineUpdateAmmount;
                    _linePointUpdateCounter = 0;
                    volumeEvaluate = Mathf.Clamp01(_soundCurve.Evaluate(_noise));
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
