using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] Vector2 _offset;

    [Header("Drop")]

    [SerializeField] Transform _player;


	void Update ()
    {
        transform.position = _player.position + (Vector3)_offset;
	}
}
