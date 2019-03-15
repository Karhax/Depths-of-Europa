using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndingScript : MonoBehaviour {

    [SerializeField] Transform _waypoint;
    [SerializeField] Transform _moveShipToPosition;
    [SerializeField, Range(0, 360)] float _moveShipRoationZ;
    [SerializeField, Range(0, 15)] float _timeToWaitToStartDialog = 1.5f;
    [SerializeField] Animator _clawAnimator;
    [SerializeField] Light _enterLight;
    [SerializeField] Color _enterLightColor = Color.white;
    [SerializeField] private string _nextScene;

    private StartDialog _dialogScript = null;
    bool _hasDialog = false;
    // [SerializeField] private SoundObject _goalReachedSound = null;

    AudioSource _clawAudio;
    bool _shipInBase = false;

    readonly float ANGLE_BACKWARD_FORWARD_ENTER_BASE = 60;
    readonly string CLAW_ANIMATOR_ENTERED_TRIGGER = "Entered Base";

    private void Awake()
    {
        _clawAudio = GetComponent<AudioSource>();
        _dialogScript = GetComponent<StartDialog>();

        if (_dialogScript != null)
            _hasDialog = true;
    }

    private void BeginEndingDialog()
    {
        GameManager.DialogStartedReaction();
        _dialogScript.DialogOverEvent += EndLevel;
        _dialogScript.StartDialogs();
    }
    public void EndLevel()
    {
        if (_hasDialog)
            _dialogScript.DialogOverEvent -= EndLevel;

        GameManager.LevelEndReached(_nextScene);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Statics.Tags.PLAYER_OUTSIDE) && !_shipInBase)
        {
            Vector3 facingBase = _waypoint.position - other.transform.position;

            if (Vector3.Angle(facingBase, other.transform.up) < ANGLE_BACKWARD_FORWARD_ENTER_BASE)
                ShipInBase(other);     
        }        
    }

    private void ShipInBase(Collider2D other)
    {
        if (_enterLight != null)
            _enterLight.color = _enterLightColor;

        _shipInBase = true;

        ShipInBase shipInBaseScript = other.GetComponent<ShipInBase>();

        if (transform.parent.parent != null)
            shipInBaseScript.InBase(true, _moveShipToPosition.position, _moveShipRoationZ + transform.parent.parent.rotation.eulerAngles.z, true);
        else
            shipInBaseScript.InBase(true, _moveShipToPosition.position, _moveShipRoationZ, true);

        PlayerGUIScript playerUI = GameManager.CameraObject.GetComponentInChildren<PlayerGUIScript>();
        playerUI.gameObject.SetActive(false);

        // Play sound here, or perhaps in the function EndLevel()

        StartCoroutine(DockingAnimation());
    }

    IEnumerator DockingAnimation()
    {
        _clawAudio.Play();
        _clawAnimator.SetTrigger(CLAW_ANIMATOR_ENTERED_TRIGGER);

        yield return new WaitForSeconds(_timeToWaitToStartDialog);

        if (_hasDialog)
            BeginEndingDialog();
        else
            EndLevel();
    }
}
