using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class AssignRandomScale : MonoBehaviour
{
    private float scale;

    // Use this for initialization
    void Awake()
    {
        scale = Random.Range(0.1f, 0.5f);
        transform.localScale = new Vector3(scale, scale, 0);
    }


}
