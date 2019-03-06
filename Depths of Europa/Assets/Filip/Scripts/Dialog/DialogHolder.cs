using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : MonoBehaviour
{
    [SerializeField] bool _isTutorial = false;
    [SerializeField] DialogBoxScriptableObject _dialogObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartDialog dialogScript = other.GetComponent<StartDialog>();

        if (dialogScript != null)
        {
            if (dialogScript.StartDialogs(_dialogObject, _isTutorial))
                Destroy(gameObject);
        }
    }
}
