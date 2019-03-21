using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    [SerializeField, Range(0, 15)] float _fadeSpeed = 4;

    public static MenuMusic _musicScript;

    private void Awake()
    {
        if (_musicScript == null)
        {
            _musicScript = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
            Destroy(gameObject);
    }

    public void FadeMusic()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        AudioSource thisAudioSource = GetComponent<AudioSource>();
        Timer fadeTimer = new Timer(_fadeSpeed);

        while (!fadeTimer.Expired())
        {
            fadeTimer.Time += Time.deltaTime;

            thisAudioSource.volume = 1 - fadeTimer.Ratio();

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
