using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LineSetup : MonoBehaviour {

    [SerializeField] GameObject LineRenderPrefab;

    [System.Serializable]
    struct LineData
    {
        [SerializeField] internal string name;
        [SerializeField] internal Object _dataFile;
        [SerializeField] internal Color _lineColour;
    }
    
    [SerializeField] LineData[] _lines;
    GameObject[] _lineObjects;
    int _validObjects;
    int[] _validIdexes;
    bool _hasSpawnedObject = false;

    private void OnValidate()
    {

        if(_hasSpawnedObject)
        {

            for(int i = 0; i < _lineObjects.Length; i++)
            {

                if(_lineObjects[i] != null)
                _lineObjects[i].GetComponent<LineRender>().DeleteThis();

            }

            _lineObjects = null;
            _hasSpawnedObject = false;

        }

        _validObjects = 0;
        _validIdexes = new int[_lines.Length];

        for(int i = 0; i < _lines.Length; i++)
        {

            try
            {
                if (_lines[i]._dataFile.name != "")
                    _lines[i].name = _lines[i]._dataFile.name;

                _validIdexes[_validObjects] = i;

                _validObjects++;
            }
            catch (System.Exception)
            {
                _lines[i].name = "";
            }

        }
        
        if (_validObjects != 0)
        {
            _lineObjects = new GameObject[_validObjects];


            for (int i = 0; i < _lineObjects.Length; i++)
            {

                _lineObjects[i] = Instantiate(LineRenderPrefab,transform);
                LineData lineDataTemp = _lines[_validIdexes[i]];
                _lineObjects[i].GetComponent<LineRender>().SetUp(lineDataTemp._dataFile, lineDataTemp._lineColour, lineDataTemp.name);

            }

            _hasSpawnedObject = true;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
