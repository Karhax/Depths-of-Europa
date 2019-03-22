using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class WaterStream : MonoBehaviour
{
    [SerializeField] Transform _spawnYLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.FLOATING_OBJECT))
            other.transform.position = new Vector3(other.transform.position.x, _spawnYLevel.transform.position.y, other.transform.position.z);
    }
}
