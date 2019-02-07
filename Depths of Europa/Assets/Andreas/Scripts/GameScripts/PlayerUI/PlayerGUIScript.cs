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
            FlareAmmountChange(_debugAmmountOfFlares);
            _debugFlareArray = false;
        }
    }

    private void OnHealthChange(float HPRatio)
    {
        _healthBarImage.fillAmount = HPRatio;
    }
}
