using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class StartTutorial : MonoBehaviour
{
    [SerializeField] Sprite _tutorialImage;

    bool _canPlay = false;
    readonly float WAIT_TIME = 0.25f;
    Collider2D _lastCollider = null;

    private void Awake()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        _canPlay = true;

        if (_lastCollider != null)
            Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_OUTSIDE))
        {
            if (!_canPlay)
                _lastCollider = other;
            else
                Play();
        }
    }

    private void Play()
    {
        GameManager.TutorialStart(_tutorialImage);
        Destroy(gameObject);
    }
}
