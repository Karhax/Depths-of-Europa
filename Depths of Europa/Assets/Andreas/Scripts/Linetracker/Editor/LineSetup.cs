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
    GameObject[] _lineObjects, _oldObjects;
    int _validObjects;
    int[] _validIdexes;
    bool _hasSpawnedObject = false, _triedToSpawnNew = false, _deletePassFinished = false, _authPassFinished = false, _pipelineActive = false;


    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (!_pipelineActive)
            {
                LineAssembly();
            }
            else
            {
                _triedToSpawnNew = true;
            }
        }
    }


    private void LineAssembly()
    {
        _triedToSpawnNew = false;
        if (!_pipelineActive)
        {
            _pipelineActive = true;
            //DeletePass();
            StartCoroutine(DeletePass());

            StartCoroutine(AuthPass());

            StartCoroutine(SpawnPass());
        }
    }



    bool isTrue()
    {
        return true;
    }

    IEnumerator DeletePass()
    {
        yield return new WaitUntil(isTrue);

        if (_hasSpawnedObject)
        {
            for (int i = 0; i < _lineObjects.Length; i++)
            {

                if (_lineObjects[i] != null)
                    _lineObjects[i].GetComponent<LineRender>().DeleteThis();

            }
            KillGarbage();


            _oldObjects = _lineObjects;
            _lineObjects = null;
            _hasSpawnedObject = false;

        }

        _validObjects = 0;
        _validIdexes = new int[_lines.Length];
        _deletePassFinished = true;
    }

    void KillGarbage()
    {
        LineRender[] children = GetComponentsInChildren<LineRender>();
        if (children != null)
        {
            for (int i = 0; i < children.Length; i++)
            {

                if (children[i] != null)
                    children[i].DeleteThis();
                
            }
        }
    }

    bool DeletePassDone()
    {
        return _deletePassFinished;
    }

    IEnumerator AuthPass()
    {
        yield return new WaitUntil(DeletePassDone);
        _deletePassFinished = false;

        for (int i = 0; i < _lines.Length; i++)
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
        _authPassFinished = true;
    }

    private bool AuthPassDone()
    {
        return _authPassFinished;
    }

    IEnumerator SpawnPass()
    {
        yield return new WaitUntil(AuthPassDone);
        _authPassFinished = false;


        _lineObjects = new GameObject[_validObjects];


        for (int i = 0; i < _lineObjects.Length; i++)
        {

                _lineObjects[i] = Instantiate(LineRenderPrefab, transform);
                LineData lineDataTemp = _lines[_validIdexes[i]];
                _lineObjects[i].GetComponent<LineRender>().SetUp(lineDataTemp._dataFile, lineDataTemp._lineColour, lineDataTemp.name);


        }


            _pipelineActive = false;
            _authPassFinished = false;
            _deletePassFinished = false;
            _hasSpawnedObject = true;
      
            Recall();
    

    }

    private void Recall()
    {
        _pipelineActive = false;
        //LateDeletePass();
        StopAllCoroutines();
        if (_triedToSpawnNew)
        {
            LineAssembly();
        }
    }
    

    void LateDeletePass()
    {
        if (_oldObjects != null)
        {
            for (int i = 0; i < _oldObjects.Length; i++)
            {

                if (_oldObjects[i] != null)
                    _oldObjects[i].GetComponent<LineRender>().DeleteThis();

            }
        }
    }
}

