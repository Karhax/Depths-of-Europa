using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class LineRender : MonoBehaviour {
    Color _colour;
    Object _dataFileReference;
    string _unparsedData;
    //Mechanically seperated for yuor pleasure!
    string[] _seperatedData;
    Vector4[] _parsedAndSeperatedData;
    int _dataReadStart;
    float _minLineWidth, _maxLineWidth;
    LineRenderer _lineRenderer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void SetUp(Object _object, Color colour, string name)
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        gameObject.name = gameObject.name + "(" + name + ")";
        _dataFileReference = _object;
        _colour = colour;
        FindReadAndParse();
        ArrangeLine();
    }

    void FindReadAndParse()
    {
        string pathWay = AssetDatabase.GetAssetPath(_dataFileReference);
        using (StreamReader Streamread = new StreamReader(pathWay))
        {
            _unparsedData = Streamread.ReadToEnd();
            Streamread.Close();
        }
        _seperatedData = _unparsedData.Split(new char[] { '|', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        _minLineWidth = float.Parse(_seperatedData[0]);
        _maxLineWidth = float.Parse(_seperatedData[1]);
        _dataReadStart = int.Parse(_seperatedData[2]);
        float trackerLenght = float.Parse(_seperatedData[_seperatedData.Length - 1]);
        int dataLenght = (_seperatedData.Length - _dataReadStart) / 4;
        _parsedAndSeperatedData = new Vector4[dataLenght];
        int k = 0;

        for (int i = _dataReadStart; i < _seperatedData.Length; i += 4)
        {
            _parsedAndSeperatedData[k].x = float.Parse(_seperatedData[i]);
            _parsedAndSeperatedData[k].y = float.Parse(_seperatedData[i+1]);
            _parsedAndSeperatedData[k].z = float.Parse(_seperatedData[i+2]);
            _parsedAndSeperatedData[k].w = float.Parse(_seperatedData[i+3]) / trackerLenght;
            k++;
        }
    }


    void ArrangeLine()
    {
        AnimationCurve animCurve = new AnimationCurve();
        int lineLenght = _parsedAndSeperatedData.Length;
        _lineRenderer.positionCount = lineLenght;
        _lineRenderer.widthMultiplier = _maxLineWidth;
        _lineRenderer.startColor = _colour;
        _lineRenderer.endColor = _colour;
        animCurve.AddKey(0, _parsedAndSeperatedData[0].z);
        for (int i = 0; i < lineLenght; i++)
        {
            _lineRenderer.SetPosition(i, new Vector2(_parsedAndSeperatedData[i].x, _parsedAndSeperatedData[i].y));
            animCurve.AddKey(_parsedAndSeperatedData[i].w, _parsedAndSeperatedData[i].z);
        }
        _lineRenderer.widthCurve = animCurve;
    }

    public void DeleteThis()
    {
        DestroyImmediate(gameObject);
    }
}
