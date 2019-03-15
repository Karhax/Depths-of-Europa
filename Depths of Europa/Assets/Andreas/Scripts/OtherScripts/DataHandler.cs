using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class DataHandler {

    private static DataStoresType[] _dataStore;
    private static DataStoresType[] _defaults = new DataStoresType[] { new DataStoresType(DataIdentifier.boolIsScreenShakeEnabled, DataType.Bool, "1") };

    public enum DataIdentifier
    {
        boolIsScreenShakeEnabled,

    }
    public enum DataType
    {
        Bool,
    }
    public class DataStoresType
    {
        public DataIdentifier _identifier;
        public DataType _dataType;
        public string _value;

        public DataStoresType(DataIdentifier identifier, DataType dataType, string value)
        {
            _identifier = identifier;
            _dataType = dataType;
            _value = value;
        }
    }
    
    
    private static DataStoresType GetData(DataIdentifier identifier, DataType dataType)
    {
        for (int i = 0; i < _dataStore.Length;i++)
        {
            if(identifier == _dataStore[i]._identifier)
            {
                if(_dataStore[i]._dataType != dataType)
                {
                    Debug.LogError("Data point " + identifier.ToString() + " not of correct data type");
                    return null;
                }
                else
                {
                    return _dataStore[i];
                }
            }
        }
         
        Debug.LogError(identifier.ToString() + " not found");
        return GetDefault(identifier, dataType);
    }

    private static DataStoresType GetDefault(DataIdentifier identifier, DataType dataType)
    {
        for (int i = 0; i < _defaults.Length; i++)
        {
            if (identifier == _defaults[i]._identifier)
            {
                if (_defaults[i]._dataType != dataType)
                {
                    Debug.LogError("Data point " + identifier.ToString() + " not of correct data type");
                    return null;
                }
                else
                {
                    return _defaults[i];
                }
            }
        }
        return null;
    }

    public static bool GetBoolData(DataIdentifier identifier)
    {
        DataStoresType data = GetData(identifier, DataType.Bool);
        if (data != null)
        {
            int value = int.Parse(data._value);
            if (value == 1)
                return true;
            else
                return false;
        }
        return false;
    }

}
