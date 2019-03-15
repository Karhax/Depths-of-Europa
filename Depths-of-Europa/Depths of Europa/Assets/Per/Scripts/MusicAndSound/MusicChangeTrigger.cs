using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour {

    [SerializeField] private MusicManagement _musicManager = null;
    [SerializeField] private bool[] _stemsToBeActive;
    [SerializeField] private bool _resetOnExit = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(Statics.Tags.PLAYER_OUTSIDE))
        {
            if (_musicManager == null)
            {
                Debug.LogWarning("A MusicChangeTrigger was activated, but it could not find any MusicManager");
            }
            else
            {
                _musicManager.AdjustVolumes(_stemsToBeActive);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Statics.Tags.PLAYER_OUTSIDE))
        {
            if (_resetOnExit && _musicManager != null)
            {
                _musicManager.ResetVolumes();
            }
        }
    }
}
