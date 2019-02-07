using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGUIScript : MonoBehaviour {

    [SerializeField] Image _healthBarImage;
    [SerializeField] float _debugCurrentHP = 100;
    float _debugMaxHP = 100;
    [SerializeField] bool _debugHPChangeCall = false;

    [SerializeField] bool _debugFlareArray = false;
    public delegate void FlareAmmountChnageHandler(int ammountOFFlares);
    public event FlareAmmountChnageHandler FlareAmmountChange;
    [SerializeField] int _debugAmmountOfFlares = 5;
    int _arrayCount = 0;
    [SerializeField] GameObject ArrayInstance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_debugHPChangeCall)
        {
            OnHealthChange(_debugCurrentHP / _debugMaxHP);
            _debugHPChangeCall = false;
        }
        if (_debugFlareArray)
        {
            OnFlare();
            _debugFlareArray = false;
        }
    }

    private void OnHealthChange(float HPRatio)
    {
        _healthBarImage.fillAmount = HPRatio;
    }

    private void OnFlare()
    {
        int tempFlareArrayCount = FlareCountCalc();
        if(tempFlareArrayCount < _arrayCount)
        {
            _arrayCount -= _arrayCount - tempFlareArrayCount;
        }
        else if(tempFlareArrayCount > _arrayCount)
        {
            int tempFlareExcess = tempFlareArrayCount - _arrayCount;
            if (tempFlareExcess == 1)
            {
                _arrayCount++;
                GameObject _newFlareArray = Instantiate(ArrayInstance, transform, false);
                _newFlareArray.GetComponent<FlareArrayScript>().SetArrayID(_arrayCount);
            }
            else if(tempFlareExcess > 1)
            {
                for(int i = 0; i < tempFlareExcess;i++)
                {
                    _arrayCount++;
                    GameObject _newFlareArray = Instantiate(ArrayInstance, transform, false);
                    _newFlareArray.GetComponent<FlareArrayScript>().SetArrayID(_arrayCount);
                }
            }
        }
        Debug.Log(tempFlareArrayCount);

        FlareAmmountChange(_debugAmmountOfFlares);
    }

    private int FlareCountCalc()
    {
        if (_debugAmmountOfFlares > 0)
            return (_debugAmmountOfFlares - 1) / 5;
        else if (_debugAmmountOfFlares <= 0)
            return 0;
        else
            return 0;
    }
}
