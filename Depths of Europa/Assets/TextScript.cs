using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class TextScript : MonoBehaviour {

    [SerializeField] AudioMixer AudioMixer;
    [SerializeField] Text Text;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Text.text = AudioMixer.GetVolumeValue("Master").ToString();
	}
}
