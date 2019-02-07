using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndingScript : MonoBehaviour {
    
    [SerializeField] private string _nextScene;

    // [SerializeField] private DialogSystem _dialogToUse = null;
    // [SerializeField] private SoundObject _goalReachedSound = null;
    
	private void Start () {
        
	}

    private void BeginEndingDialog()
    {
        // Call the beginning of the connected dialog system
        // For testing purposes:
        EndLevel();
    }
    public void EndLevel()
    {
        // Function that the connected dialog system should call to end the level.
        // Call the GameManager and tell it to end the level.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Statics.Tags.PLAYER_OUTSIDE))
        {
            MoveShip shipMovement = other.GetComponent<MoveShip>();
            // _shipMovement.EnableInput(); NOT IMPLEMENTED

            // Call function in the base object that plays the docking animation

            // Play sound here, or perhaps in the function EndLevel()

            BeginEndingDialog();
        }
    }
}
