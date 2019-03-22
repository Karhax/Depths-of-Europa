using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class EnableDisableStream : MonoBehaviour
{
    [SerializeField] bool _reverseOnExit = false;
    [SerializeField] bool _enable = true;
    [SerializeField] GameObject _stream;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_OUTSIDE))
            _stream.SetActive(_enable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_OUTSIDE) && _reverseOnExit)
            _stream.SetActive(!_enable);
    }
}
