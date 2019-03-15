using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    public static int FindResolutionInArray(this Resolution[] resolutions, Resolution resolution)
    {
        for(int i = 0; i < resolutions.Length; i++)
        {
            if(resolutions[i].width == resolution.width && resolutions[i].height == resolution.height)
            {
                return i;
            }
        }
        return -1;
    }
    public static string SliderValueToPercentString(this Slider slider)
    {
        int percentage = Mathf.FloorToInt(slider.value != 0 ? slider.value * (100 / slider.maxValue):0);
        return percentage + "%";
    }
    public static string ValueToPercentString(this float value, float maxValue)
    {
        int percentage = Mathf.FloorToInt(value!=0?value * (100 / maxValue):0);
        return percentage + "%";
    }
    public static float GetVolumeValue(this AudioMixer audioMixer,string name)
    {
        float value;
        bool result = audioMixer.GetFloat(name, out value);
        return result ? value : -1;
    }
    public static float GetVolumeValue(this AudioMixer audioMixer, string name, float min, float max)
    {
        float value;
        bool result = audioMixer.GetFloat(name, out value);
        value = Mathf.InverseLerp(min, max, value);
        return result ? value : -1;
    }
    public static string FloatToSecondsRemaining(this float value)
    {
        return "Reverting in " + Mathf.FloorToInt(value) + " Seconds";
    }
    public static float SliderToGamma(this Slider slider)
    {
        //Debug.Log(slider.value);
        if (slider.value > 0)
        {
            //Debug.Log(Mathf.Lerp(0, 2, slider.value));
            return Mathf.Lerp(0, 2, slider.value);
        }
        else if (slider.value < 0)
        {
            //  Debug.Log(Mathf.Lerp(0f ,- 1.5f,Mathf.Abs(slider.value)));
            //Debug.Log(Mathf.Lerp(0f, -1f, Mathf.Abs(slider.value)));
            return Mathf.Lerp(0f ,- 1f,Mathf.Abs(slider.value));
        }
        else
        //Debug.Log(slider.value);
            return slider.value;
    }
    public static void GammaToSlider(this Slider slider, float value)
    {
        //Debug.Log(slider.value);
        if (value > 0)
        {
            //  Debug.Log(Mathf.InverseLerp(0, 2, value));

            slider.value = Mathf.InverseLerp(0, 2, value);
           // Debug.Log(slider.value);
        }
        else if (value < 0)
        {
            //Debug.Log(-Mathf.InverseLerp(0,1.5f,Mathf.Abs(value)));
            //  Debug.Log(Mathf.Lerp(0f ,- 1.5f,Mathf.Abs(slider.value)));
            slider.value = -Mathf.InverseLerp(0, 1f ,Mathf.Abs(value));
            //Debug.Log(slider.value);
        }
        else
            //Debug.Log(slider.value);
            slider.value = 0;

        //Debug.LogError(-Mathf.InverseLerp(0, 1.5f, Mathf.Abs(value)));
    }
    public static float GammaToSlider(this float value)
    {
        //Debug.Log(slider.value);
        if (value > 0)
        {
            //  Debug.Log(Mathf.InverseLerp(0, 2, value));

            return Mathf.InverseLerp(0, 2, value);
            // Debug.Log(slider.value);
        }
        else if (value < 0)
        {
            //Debug.Log(-Mathf.InverseLerp(0,1.5f,Mathf.Abs(value)));
            //  Debug.Log(Mathf.Lerp(0f ,- 1.5f,Mathf.Abs(slider.value)));
            return -Mathf.InverseLerp(0, 1f, Mathf.Abs(value));

            //Debug.Log(slider.value);
        }
        else
            //Debug.Log(slider.value);
            return 0;

        //Debug.LogError(-Mathf.InverseLerp(0, 1.5f, Mathf.Abs(value)));
    }
}
