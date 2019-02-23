using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScreenOverlaySize : MonoBehaviour
{
    [SerializeField] CanvasScaler _scaler;
    [SerializeField] RectTransform _overlay;

    private void Awake()
    {
        _scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        _overlay.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}
