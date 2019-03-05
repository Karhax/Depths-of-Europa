using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
public class ShipTrackerScript : MonoBehaviour
{

    [SerializeField, Range(0.1f, 2)] float _pollingIntervall = 0.5f;
    [SerializeField, Range(0.25f, 5)] float _distanceThreashold = 1;
    [SerializeField, Range(1, 10)] float _maxLineWidth = 10;
    [SerializeField, Range(0, 1)] float _minimumLineWidth  = 0.1f;

    float _lastTime = 0, _totalDistance;
    Timer _pollingCounter = new Timer(1);
    string _fileName, _path;
    Vector2 _lastPosition;
    DirectoryInfo _directoryInfo;
    bool _firstWrite = true;
    readonly int DATA_READ_START_INDEX = 3;
    //readonly int IGNORE_POSITION = 4;

    bool _fileCreated = false;
    // Use this for initialization
    void Start()
    {
        _lastPosition = transform.position;
        _pollingCounter.Duration = _pollingIntervall;
        FileCreation();
    }

    // Update is called once per frame
    void Update()
    {
        _lastTime += Time.deltaTime;
        _pollingCounter.Time += Time.deltaTime;
        if (_pollingCounter.Expired())
        {
            _pollingCounter.Reset();
            _pollingCounter.Duration = _pollingIntervall;
            float currentDistance = Vector2.Distance(transform.position, _lastPosition);
            if (currentDistance >= _distanceThreashold)
            {
                _totalDistance += currentDistance;
                float timeThiccness = _lastTime / currentDistance;
                if (timeThiccness >= _maxLineWidth)
                {
                    timeThiccness = _maxLineWidth;
                }
                else if (timeThiccness < _minimumLineWidth)
                {
                    timeThiccness = _minimumLineWidth;
                }
                else if (float.IsNaN(timeThiccness))
                {
                    timeThiccness = _minimumLineWidth;
                }
                if (_fileCreated)
                {

                    using (TextWriter ActionWriter = new StreamWriter(_fileName, true))
                    {
                        ActionWriter.Write(_lastPosition.x + "," + _lastPosition.y + "," + (timeThiccness/_maxLineWidth) + "," + _totalDistance + "|");
                        ActionWriter.Close();
                    }
                }
                _lastPosition = transform.position;
                _lastTime = 0;


            }
        }
    }
   public void FileCreation()
    {
        string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
        string dirPath = DirectoryPath();
        _fileName = basePath + dirPath + NameAssembly();

        _path = basePath + dirPath;
        if (!Directory.Exists(_path))
        {

            _directoryInfo = new DirectoryInfo(_path);
            _directoryInfo.Create();
        }
        _fileCreated = true;
        if (_firstWrite)
        {
            _firstWrite = false;
            using (TextWriter ActionWriter = new StreamWriter(_fileName, true))
            {
                ActionWriter.Write(_minimumLineWidth + "," + _maxLineWidth + "," +  DATA_READ_START_INDEX + "|");
                ActionWriter.Close();
            }
        }
    }
    private string DirectoryPath()
    {
        Scene scene = SceneManager.GetActiveScene();
        return @"\DOEPositionLogs\" + scene.name + "(" + scene.buildIndex + @")\";
    }
    private string NameAssembly()
    {
        string month = ((DateTime.Now.Month > 10) ? "" : "0") + DateTime.Now.Month;
        string day = ((DateTime.Now.Day > 10) ? "" : "0") + DateTime.Now.Day;
        string hour = ((DateTime.Now.Hour > 9) ? "" : "0") + DateTime.Now.Hour;
        string minute = ((DateTime.Now.Minute > 10) ? "" : "0") + DateTime.Now.Minute;
        string second = ((DateTime.Now.Second > 10) ? "" : "0") + DateTime.Now.Second;

        return DateTime.Now.Year + "" + month + "" + day + " at " + hour + "：" + minute + "：" + second + ".DOEPLF";
    }
}