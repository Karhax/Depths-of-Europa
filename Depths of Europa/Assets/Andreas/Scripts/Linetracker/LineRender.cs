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
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUp(Object _object, Color colour, string name)
    {
        gameObject.name = gameObject.name + "(" + name + ")";
        _dataFileReference = _object;
        _colour = colour;
        FindReadAndParse();
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
        int dataLenght = (_seperatedData.Length - _dataReadStart) / 4;
        _parsedAndSeperatedData = new Vector4[dataLenght];
        int k = 0;
        for (int i = _dataReadStart; i < _seperatedData.Length; i += 4)
        {
            _parsedAndSeperatedData[k].x = float.Parse(_seperatedData[i]);
            _parsedAndSeperatedData[k].y = float.Parse(_seperatedData[i+1]);
            _parsedAndSeperatedData[k].z = float.Parse(_seperatedData[i+2]);
            _parsedAndSeperatedData[k].w = float.Parse(_seperatedData[i+3]);
            k++;
        }
    }

    public void DeleteThis()
    {
        StartCoroutine(DeleteMe());
    }

    bool Bolians()
    {
        return true;
    }
    IEnumerator DeleteMe()
    {
        yield return new WaitUntil(Bolians);
        DestroyImmediate(this.gameObject);
    }
}
