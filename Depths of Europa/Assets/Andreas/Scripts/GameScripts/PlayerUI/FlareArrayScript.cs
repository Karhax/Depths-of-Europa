using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlareArrayScript : MonoBehaviour {
    
    [SerializeField] int _arrayID;
    [SerializeField] GameObject[] _imageArray = new GameObject[5];
    PlayerGUIScript Parent;


    private void Awake()
    {
        Parent = GetComponentInParent<PlayerGUIScript>();
        Parent.FlareAmmountChange += OnFlareAmmountChange;
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnFlareAmmountChange(int ammountOfFlares)
    {
        int showAmmount = FlareCalc(ammountOfFlares);
        if(_arrayID != 0 && showAmmount < 0)
        {
            //TODO: selfdelete function
        }
        else
        {
            for(int i = 0; i < _imageArray.Length; i++)
            {
                if (i < showAmmount)
                {
                    _imageArray[i].SetActive(true);
                }
                else
                    _imageArray[i].SetActive(false);
            }
        }
    }

    private int FlareCalc(int ammountOfFlares)
    {
        return ammountOfFlares - (_arrayID * 5);
    }
}
