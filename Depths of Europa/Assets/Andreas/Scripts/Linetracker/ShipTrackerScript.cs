using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class ShipTrackerScript : MonoBehaviour
{

    [SerializeField, Range(0.1f, 2)] float _pollingIntervall = 0.5f;
    [SerializeField, Range(0.25f, 5)] float _distanceThreashold = 1;
    [SerializeField, Range(1, 10)] float _maxLineWidth = 10;
    [SerializeField, Range(0, 1)] float _minimumLineWidth  = 0.1f;

    float _pollingCounter, _lastTime = 0;
    string _fileName, _path;
    Vector2 _lastPosition;
    DirectoryInfo _directoryInfo;
    //StreamWriter ActionWriter;
    bool _fileCreated = false;
    // Use this for initialization
    void Start()
    {
        _lastTime = Time.time;
        _lastPosition = transform.position;
        FileCreation();
    }

    // Update is called once per frame
    void Update()
    {

        _pollingCounter += Time.deltaTime;
        if (_pollingCounter >= _pollingIntervall)
        {
            _pollingCounter = 0;
            float currentDistance = Vector2.Distance(transform.position, _lastPosition);
            if (currentDistance <= _distanceThreashold)
            {
                float timeThiccness = Time.time - _lastTime / currentDistance;
                if (timeThiccness >= _maxLineWidth)
                {
                    timeThiccness = _maxLineWidth;
                }
                else if(timeThiccness < _minimumLineWidth)
                {
                    timeThiccness = _minimumLineWidth;
                }
                else if(float.IsNaN(timeThiccness))
                {
                    timeThiccness = _minimumLineWidth;
                }
                if (_fileCreated)

                    using (StreamWriter ActionWriter = new StreamWriter(_fileName,true))
                    {
                       // ActionWriter.Write(_lastPosition.x + "," + _lastPosition.y + "," + timeThiccness + "|");
                        ActionWriter.Close();
                    }
                
                _lastPosition = transform.position;
                _lastTime = Time.time;
            }

        }
    }
   public void FileCreation()
    {
        string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
        //Debug.Log(basePath);
        _fileName = basePath + @"\DOEPositionLogs\" + DateTime.Now.Year + "" 
            + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" 
            + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + DateTime.Now.Millisecond + ".BB";

        _path = basePath + @"\DOEPositionLogs\";
        if (!Directory.Exists(_path))
        {

            _directoryInfo = new DirectoryInfo(_path);
            _directoryInfo.Create();
        }

        if (!File.Exists(_fileName))
        {
            FileInfo file = new FileInfo(_fileName);
            file.CreateText();
            
        }
        _fileCreated = true;
    }
}