using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMeterScript : MonoBehaviour {

    [SerializeField] Rect _indicatorBounds;
    RectTransform _rectTransform;

    [SerializeField, Range(0, 1)] float _debugLineModifier;
    [SerializeField] float _frequencyModifier = 10, _magnitudeModifier = 10, _traversalSpeed = 0.2f;


    int _oldLineCount, _linePointUpdateCounter, _lineUpdateTarget;
    private IEnumerator _lineUpdate;
    float _yCenterInterpolator = 0, _traversal;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _lineUpdate = LineRender();
    }

    private void OnValidate()
    {

    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(_lineUpdate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator LineRender()
    {
        while (true)
        {
            for (int i = 0; i < 1; i++)
            {


                _traversal += _traversalSpeed * Time.deltaTime;




                float xPositionBase = _rectTransform.position.x + _indicatorBounds.x;

                float xPointPosition = Mathf.Lerp(xPositionBase - _indicatorBounds.width, xPositionBase + _indicatorBounds.width, 1);

                float yPositionBase = _rectTransform.position.y + _indicatorBounds.y;

                float yPointCalc = (Mathf.Sin(_yCenterInterpolator * _debugLineModifier * Mathf.PI) * _magnitudeModifier * Mathf.Sin((_frequencyModifier * Mathf.PI) + (_traversal * Mathf.PI)));




                float normalizedYPoint = Mathf.InverseLerp(yPositionBase - _indicatorBounds.height, yPositionBase + _indicatorBounds.height, yPointCalc);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
