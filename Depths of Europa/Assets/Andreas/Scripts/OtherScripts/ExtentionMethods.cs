using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionMethods {

    public static int AddAndRepeatInt(this int adder, int roof)
    {
        adder++;
        return adder = Mathf.FloorToInt(Mathf.Repeat(adder, roof));

    }
}
