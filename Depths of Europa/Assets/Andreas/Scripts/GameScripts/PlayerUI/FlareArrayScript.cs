using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlareArrayScript : MonoBehaviour {
    
    [SerializeField] int _arrayID;
    [SerializeField] GameObject[] _imageArray = new GameObject[5];
    PlayerGUIScript Parent;
    readonly int FIRST_ARRAY = 0;
    private RectTransform RectTransform;
    protected static Vector2 BASE_POSITION = new Vector2(0,0);
    readonly float X_POSITION_INCREMENT = 25f;

    private void Awake()
    {
        
        RectTransform = GetComponent<RectTransform>();

        if (_arrayID == 0 && BASE_POSITION.x == 0)
        {
            BASE_POSITION = RectTransform.anchoredPosition;
        }
        Parent = GetComponentInParent<PlayerGUIScript>();
        Parent.FlareAmmountChange += OnFlareAmmountChange;
        
    }

    private void OnEnable()
    {
        if (Parent != null)
            Parent.FlareAmmountChange += OnFlareAmmountChange;
    }

    private void OnDisable()
    {
        if (Parent != null)
            Parent.FlareAmmountChange -= OnFlareAmmountChange;
    }
    // Use this for initialization
    void Start ()
    {
        RectTransform.anchoredPosition = new Vector2(BASE_POSITION.x + _arrayID * X_POSITION_INCREMENT, BASE_POSITION.y);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Code that checks ammount of flares that is apropriate to be displayed by this object and if less than 0 and isnt base object then delete it self
    /// </summary>
    /// <param name="ammountOfFlares">Ammount of current flares</param>
    private void OnFlareAmmountChange(int ammountOfFlares)
    {
        int showAmmount = FlareCalc(ammountOfFlares);
        if(showAmmount < 0 && _arrayID != FIRST_ARRAY)
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

    /// <summary>
    /// Checks the ammount of flares apropriate for this object to display
    /// </summary>
    /// <param name="ammountOfFlares">Ammount of flares</param>
    /// <returns></returns>
    private int FlareCalc(int ammountOfFlares)
    {
        return ammountOfFlares - (_arrayID * _imageArray.Length);
    }

    /// <summary>
    /// Handles object destruction
    /// </summary>
    private void DeletThis()
    {
        Parent.FlareAmmountChange -= OnFlareAmmountChange;
        Destroy(gameObject);
    }

    /// <summary>
    /// Public function for setting object ArrayID at creation
    /// </summary>
    /// <param name="newArrayID">Array ID to be assigned to this object</param>
    public void SetArrayID(int newArrayID)
    {
        _arrayID = newArrayID;
    }
}
