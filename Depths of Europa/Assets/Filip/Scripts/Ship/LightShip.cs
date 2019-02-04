using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class LightShip : MonoBehaviour
{
    [Header("Drop")]

    [SerializeField] GameObject _lightHolder;
    [SerializeField] GameObject _headlight;

    private void Update()
    {
        bool onOff = Input.GetButtonDown(GameInput.ACTION2);
        bool headLight = Input.GetButtonDown(GameInput.HEADLIGHT);

        if (onOff)
            _lightHolder.SetActive(!_lightHolder.activeSelf);

        if (headLight && _lightHolder.activeSelf)
            _headlight.SetActive(!_headlight.activeSelf);
    }
}
