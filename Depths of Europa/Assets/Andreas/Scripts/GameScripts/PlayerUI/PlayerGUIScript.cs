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

    DamageShip _damageShipScript;
    LightShip _lightShipScript;

    // Use this for initialization
    void Start()
    {
        _damageShipScript = GameManager.ShipObject.GetComponent<DamageShip>();
        _lightShipScript = GameManager.ShipObject.GetComponent<LightShip>();
        _damageShipScript.ShipTakeDamageEvent += OnHealthChange;
        _lightShipScript.ShipUsedFlareEvent += OnFlare;
    }

    private void OnEnable()
    {
        if (_damageShipScript != null)
            _damageShipScript.ShipTakeDamageEvent += OnHealthChange;
        if (_lightShipScript != null)
            _lightShipScript.ShipUsedFlareEvent += OnFlare;
    }

    private void OnDisable()
    {
        if (_damageShipScript != null)
            _damageShipScript.ShipTakeDamageEvent -= OnHealthChange;
        if (_lightShipScript != null)
            _lightShipScript.ShipUsedFlareEvent -= OnFlare;
    }
	

    private void OnHealthChange(float HPRatio)
    {
        _healthBarImage.fillAmount = HPRatio;
    }

    private void OnFlare(int amountOfFlares)
    {
        Debug.Log(amountOfFlares);

        int tempFlareArrayCount = FlareCountCalc(amountOfFlares);
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

        FlareAmmountChange(amountOfFlares);
    }

    private int FlareCountCalc(int amountOfFlares)
    {
        if (amountOfFlares > 0)
            return (amountOfFlares - 1) / 5;
        else if (amountOfFlares <= 0)
            return 0;
        else
            return 0;
    }
}
