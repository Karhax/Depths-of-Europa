using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRestartScript : MonoBehaviour {

    public delegate void RestartTestDelegate();
    public event RestartTestDelegate RestartDetector;

	private void OnTriggerEnter2D (Collider2D other) {
        if (RestartDetector != null && other.gameObject.CompareTag(Statics.Tags.PLAYER_OUTSIDE))
        {
            RestartDetector();
        }
    }
}
