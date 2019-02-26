using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFishRemovalDetection : MonoBehaviour {

    public delegate void AreaExitDelegate();
    public event AreaExitDelegate ActiveAreaExit;

	private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Statics.Tags.IN_CAMERA_TRIGGER))
        {
            if(ActiveAreaExit != null)
            {
                ActiveAreaExit();
            }
        }
    }
}
