using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlareArrayScript : MonoBehaviour {
    
    [SerializeField] int _arrayID;
    [SerializeField] GameObject[] _imageArray = new GameObject[5];
    PlayerGUIScript Parent;
    readonly int FIRST_ARRAY = 0;
    private RectTransform RectTransform;
    readonly Vector2 BASE_POSITION = new Vector2(25, 20);
    readonly float X_POSITION_INCREMENT = 25f;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Parent = GetComponentInParent<PlayerGUIScript>();
        Parent.FlareAmmountChange += OnFlareAmmountChange;
        
    }

    // Use this for initialization
    void Start ()
    {
        RectTransform.anchoredPosition = new Vector2(BASE_POSITION.x + _arrayID * X_POSITION_INCREMENT, BASE_POSITION.y);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnFlareAmmountChange(int ammountOfFlares)
    {
        int showAmmount = FlareCalc(ammountOfFlares);
        if(_arrayID != FIRST_ARRAY && showAmmount < 0)
        {
            DeletThis();
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
        return ammountOfFlares - (_arrayID * _imageArray.Length);
    }

    private void DeletThis()
    {
        Parent.FlareAmmountChange -= OnFlareAmmountChange;
        Destroy(gameObject);
    }

    public void SetArrayID(int newArrayID)
    {
        _arrayID = newArrayID;
    }
}
