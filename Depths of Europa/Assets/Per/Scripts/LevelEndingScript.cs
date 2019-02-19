using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndingScript : MonoBehaviour {

    public delegate void DelegateLevelEnding(string sceneName);
    public event DelegateLevelEnding LevelEndingDetected;
    
    [SerializeField] private string _nextScene;

    private StartDialog _dialogScript = null;
    bool _hasDialog = false;
    // [SerializeField] private SoundObject _goalReachedSound = null;

    bool _shipInBase = false;

    private void Awake()
    {
        _dialogScript = GetComponent<StartDialog>();

        if (_dialogScript != null)
            _hasDialog = true;
    }

    private void BeginEndingDialog()
    {
        _dialogScript.DialogOverEvent += EndLevel;
        _dialogScript.StartDialogs();
    }
    public void EndLevel()
    {
        if (_hasDialog)
            _dialogScript.DialogOverEvent -= EndLevel;

        if (LevelEndingDetected != null)
        {
            LevelEndingDetected(_nextScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Statics.Tags.PLAYER_OUTSIDE) && !_shipInBase)
            ShipInBase(other);
    }

    private void ShipInBase(Collider2D other)
    {
        _shipInBase = true;

        ShipInBase shipInBaseScript = other.GetComponent<ShipInBase>();
        shipInBaseScript.InBase(true);

        PlayerGUIScript playerUI = GameManager.CameraObject.GetComponentInChildren<PlayerGUIScript>();
        playerUI.gameObject.SetActive(false);

        // Call function in the base object that plays the docking animation

        // Play sound here, or perhaps in the function EndLevel()

        if (_hasDialog)
            BeginEndingDialog();
        else
            EndLevel();
    }
}
