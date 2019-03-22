using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class AssignRandomScale : MonoBehaviour
{

    [SerializeField, Range(0,10)] float MaxScale;
    [SerializeField, Range(0, 10)] float MinScale;
    // Use this for initialization
    void Awake()
    {
        float scale = Random.Range(MinScale, MaxScale);
        transform.localScale = new Vector3(scale, scale, 0);
    }


}
