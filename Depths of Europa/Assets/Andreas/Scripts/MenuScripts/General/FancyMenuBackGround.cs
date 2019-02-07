using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FancyMenuBackGround : MonoBehaviour
{
    Timer TimeDown = new Timer(1);
    Camera _cam;
    Color _oldCol, _newCol;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _oldCol = _cam.backgroundColor;
        _newCol = RandomColor();
        TimeDown.Duration = Random.Range(0.5f, 4);
    }


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeDown.Time += Time.deltaTime;
        _cam.backgroundColor = Color.Lerp(_oldCol, _newCol, TimeDown.Ratio());
        if(TimeDown.Ratio() >= 0.98f)
        {
            NewColor();
        }
    }

    private void NewColor()
    {
        _oldCol = _newCol;
        _newCol = RandomColor();
        TimeDown.Reset();
        TimeDown.Duration = Random.Range(0.5f, 4);
    }

    /// <summary>
    /// Returns a random colour
    /// </summary>
    /// <returns></returns>
    private static Color RandomColor()
    {
        Color RandomColor = new Color32(
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255));
        return RandomColor;
    }
}
