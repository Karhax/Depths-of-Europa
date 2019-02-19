using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionMethods {

    public static int AddAndRepeatInt(this int adder, int roof)
    {
        adder++;
        return adder = Mathf.FloorToInt(Mathf.Repeat(adder, roof));
    }
    public static int ModifyAndRepeatInt(this int adder, int ammount, int roof)
    {
        adder += ammount;
        return adder = Mathf.FloorToInt(Mathf.Repeat(adder, roof));
    }
    public static float ModifyAndRepeatFloat(this float adder, float ammount, float roof)
    {
        adder += ammount;
        return adder = Mathf.Repeat(adder, roof);
    }
}
