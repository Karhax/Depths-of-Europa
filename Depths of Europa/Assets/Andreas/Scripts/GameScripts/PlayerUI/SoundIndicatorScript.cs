using System.Collections;
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
    [SerializeField, Tooltip("The Colour the line should take on based on current volume, 0.0 for minimum volume and 1.0 for maximum volume")] Gradient _volumeColourGradient;
    //[SerializeField, Tooltip("The alpha value the line should take on based on when it was last updated, 0.0 for most recently updated and 1.0 for least recently updated")] Gradient _alphaGradient;

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

    float _colourUpdateFraction;

    [SerializeField,Range(0,1)] float _noise = 0.2f;

    PlayerGUIScript PGUIS;
    #endregion

    #region Read Only Variables

    readonly float SIN_BOUNDS = 1;

    readonly int MAX_GRADIENT_KEYS = 8, MAX_GRADIENT_INDEX = 7;

    #endregion



    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        //_soundIndicatorCapture = GetComponentInChildren<Camera>();
        PGUIS = FindObjectOfType<PlayerGUIScript>();
        PGUIS.NoiseAmmountChange += OnNoiseChange;
        _lineRenderer.positionCount = _numberOfLineSegments;
        _oldLineCount = _numberOfLineSegments;
        _lineWidth = _lineRenderer.widthMultiplier;
        _lineRenderer.widthMultiplier = 0;
        GenerateLineData();
        _lineUpdate = LineRender();
        _colourUpdateFraction = Mathf.InverseLerp(0, MAX_GRADIENT_INDEX, 1);
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

    /// <summary>
    /// Function called by event to update the nois level
    /// </summary>
    /// <param name="newNoise">The new noise level to be set</param>
    private void OnNoiseChange(float newNoise)
    {
        _noise = newNoise;
    }

    // Start function used to initialise the coroutine
    void Start () {
        StartCoroutine(_lineUpdate);
	}
	

    /// <summary>
    /// Function for generating a vecto 4 containing all relevant information regarding line positions
    /// </summary>
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

    /// <summary>
    /// Call to update the color graient of the line with a new colour corresponding to the volume
    /// </summary>
    /// <param name="volume">The volume value from the volume anim curve</param>
    /// <param name="drawHeadPos">Value between 0 and 1 corresponding to the draw points current position (_lineData[i].w)</param>
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

            assignedPos = drawHeadPos;
            
            SetGradientTime(headerIndex, assignedPos);
            _internalColourKeys[headerIndex].color = GetVolymeColour(volume);
        }
        /*int alphaIndex = headerIndex;
        float alphaFalloff = 0;
        for (int i = 0; i < MAX_GRADIENT_KEYS; i++)
        {
            _internalAlphaKeys[alphaIndex].alpha = GetAlphaKey(alphaFalloff);
            alphaFalloff += _colourUpdateFraction;
            alphaIndex.ModifyAndRepeatInt(-1, MAX_GRADIENT_INDEX);
        }
        */
        
        //_lineRenderer.colorGradient.colorKeys = _internalColourKeys;
        _internalColourGradient.SetKeys(_internalColourKeys, _internalAlphaKeys);
        _lineRenderer.colorGradient = _internalColourGradient;
    }

    /// <summary>
    /// Positions out a given key on the given position in the gradient
    /// </summary>
    /// <param name="i">The Position of the key in the array</param>
    /// <param name="time">The "time" position that the key should be placed on</param>
    private void SetGradientTime(int i,float time)
    {
        _internalColourKeys[i].time = time;
        //_internalAlphaKeys[i].time = time;
    }

    /// <summary>
    /// Generates valid colour gradient components
    /// </summary>
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

    /// <summary>
    /// Fetches the colour corresponding to the current volume level
    /// </summary>
    /// <param name="volume">The current volume value from the voule anim curve</param>
    /// <returns></returns>
    private Color GetVolymeColour(float volume)
    {
        return _volumeColourGradient.Evaluate(volume);
    }

    /*private float GetAlphaKey(float pos)
    {
        return _alphaGradient.Evaluate(pos).a;
    }*/

    /// <summary>
    /// Running coroutine that draws out the line
    /// </summary>
    /// <returns></returns>
    private IEnumerator LineRender()
    {
        float volumeEvaluate;
        volumeEvaluate = Mathf.Clamp01(_soundCurve.Evaluate(_noise));
        GenerateColourProfile(volumeEvaluate, 0);
        while (true)
        {
            GenerateLineData();
            _lineUpdateTarget = _lineUpdateAmmount;
            _linePointUpdateCounter = 0;
            volumeEvaluate = Mathf.Clamp01(_soundCurve.Evaluate(_noise));

            for (int i = 0; i < _numberOfLineSegments; i++)
            {
                _linePointUpdateCounter++;

                float normalYPoint = _lineData[i].z;

                float YPosCalc = Mathf.Lerp(_lineData[i].y - (_indicatorBounds.height * volumeEvaluate), _lineData[i].y + (_indicatorBounds.height * volumeEvaluate), normalYPoint);

                _lineRenderer.SetPosition(i, new Vector2(_lineData[i].x,YPosCalc));

                if (_linePointUpdateCounter >= _lineUpdateTarget)
                {
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
