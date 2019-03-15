using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class WaterStreamDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.FLOATING_OBJECT))
            Destroy(other.gameObject);
    }
}
