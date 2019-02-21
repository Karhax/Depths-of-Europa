using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : MonoBehaviour
{
    [SerializeField] DialogBoxScriptableObject _dialogObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartDialog dialogScript = other.GetComponent<StartDialog>();

        if (dialogScript != null)
            dialogScript.StartDialogs(_dialogObject);
    }
}
