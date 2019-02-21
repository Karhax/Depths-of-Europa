using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGUIScript : MonoBehaviour {

    [SerializeField] Image _healthBarImage;
    public delegate void FlareAmmountChangeHandler(int ammountOFFlares);
    public event FlareAmmountChangeHandler FlareAmmountChange;
    public delegate void NoiseChangeHandler(float noiseAmmount);
    public event NoiseChangeHandler NoiseAmmountChange;
    int _arrayCount = 0;
    [SerializeField] GameObject ArrayInstance;

    readonly int FLARE_ARRAY_LENGHT = 5;

    DamageShip _damageShipScript;
    LightShip _lightShipScript;
    MoveShip _moveShipScript;

    void Start()
    {
        _damageShipScript = GameManager.ShipObject.GetComponent<DamageShip>();
        _lightShipScript = GameManager.ShipObject.GetComponent<LightShip>();
        _moveShipScript = GameManager.ShipObject.GetComponent<MoveShip>();
        _damageShipScript.ShipTakeDamageEvent += OnHealthChange;
        _lightShipScript.ShipUsedFlareEvent += OnFlare;
        _moveShipScript.ShipSoundEvent += OnNoiseChange;
    }

    private void OnEnable()
    {
        if (_damageShipScript != null)
            _damageShipScript.ShipTakeDamageEvent += OnHealthChange;
        if (_lightShipScript != null)
            _lightShipScript.ShipUsedFlareEvent += OnFlare;
        if (_moveShipScript != null)
            _moveShipScript.ShipSoundEvent += OnNoiseChange;
    }

    private void OnDisable()
    {
        if (_damageShipScript != null)
            _damageShipScript.ShipTakeDamageEvent -= OnHealthChange;
        if (_lightShipScript != null)
            _lightShipScript.ShipUsedFlareEvent -= OnFlare;
        if (_moveShipScript != null)
            _moveShipScript.ShipSoundEvent -= OnNoiseChange;
    }
	

    private void OnHealthChange(float HPRatio)
    {
        _healthBarImage.fillAmount = HPRatio;
    }

    private void OnNoiseChange(float noise)
    {
        NoiseAmmountChange(noise);
    }

    /// <summary>
    /// Calculates the ammount of flare array object that should exist
    /// </summary>
    /// <param name="amountOfFlares">The ammount of flares</param>
    private void OnFlare(int amountOfFlares)
    {
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

    /// <summary>
    /// Calculates the ammount of Flare Array object that should exist
    /// </summary>
    /// <param name="amountOfFlares">Ammount of flares</param>
    /// <returns></returns>
    private int FlareCountCalc(int amountOfFlares)
    {
        if (amountOfFlares > 0)
            return (amountOfFlares - 1) / FLARE_ARRAY_LENGHT;
        else if (amountOfFlares <= 0)
            return 0;
        else
            return 0;
    }
}
