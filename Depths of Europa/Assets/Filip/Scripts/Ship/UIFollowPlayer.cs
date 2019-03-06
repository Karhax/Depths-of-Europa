using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField] Vector3 _offset;

    [Header("Drop")]

    [SerializeField] Transform _player;


	void Update ()
    {
        transform.position = _player.position + _offset;
	}
}
