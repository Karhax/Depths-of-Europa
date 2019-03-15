using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseControlInterface : MonoBehaviour {
    
    public abstract void BeginDockingAnimation();
    // Inte helt säker på vad denna funktion behöver. Möjligtvis behöver den
    // full tillgång till playerobject för att kunna flytta båten till kajen och
    // vrida båten till rätt vinkel. Funktionen behöver även kunna anropa LevelEndScript
    // för att markera när animationen är slut.
}
