using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class AssignRandomRotation : MonoBehaviour
{

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180, 180));
    }


}
