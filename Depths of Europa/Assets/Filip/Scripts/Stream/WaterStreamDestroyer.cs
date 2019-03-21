using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class WaterStreamDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.GetComponent<WaterStream>());
    }
}
