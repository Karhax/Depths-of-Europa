using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseV1Controller : BaseControlInterface {

    // Efterom det finns många olika typer av baser kan det vara nödvändigt att använda flera olika script
    // för att sköta alla animationer. För att LevelEndingScript ska kunna anropa dessa script på ett enkelt
    // sätt är de satta som polymorfa barn till ett gemensamt interface.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void BeginDockingAnimation()
    {

    }
}
