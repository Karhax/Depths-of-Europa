using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusicParent : MonoBehaviour {

    private static MainMusicParent singletonMusicParent;

    [SerializeField] private MusicManagement _levelMusicManager = null;
    [SerializeField] private MusicManagement _baseMusicManager = null;
    
	private void Awake () {
		if (singletonMusicParent == null)
        {
            singletonMusicParent = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (_levelMusicManager == null)
        {
            Debug.LogWarning("MainMusicParent did not have any LevelMusicManager.");
        }
        if (_baseMusicManager == null)
        {
            Debug.LogWarning("MainMusicParent did not have any BaseMusicManager.");
        }
	}

    private void Start()
    {
        if (_levelMusicManager != null)
        {
            _levelMusicManager.gameObject.SetActive(true);
        }
        if (_baseMusicManager != null)
        {
            _baseMusicManager.gameObject.SetActive(false);
        }
    }
	
    public void ActivateBaseMusic()
    {
        if (_levelMusicManager != null)
        {
            _levelMusicManager.EndMusic();
        }
        if (_baseMusicManager != null)
        {
            _baseMusicManager.gameObject.SetActive(true);
        }
    }

    public void EndBaseMusic()
    {
        if (_baseMusicManager != null)
        {
            _baseMusicManager.EndMusic();
        }
    }

    public void StopAllMusic()
    {
        if (_baseMusicManager != null)
        {
            if (_baseMusicManager.gameObject.activeSelf)
            {
                _baseMusicManager.EndMusic();
            }
        }
        if (_levelMusicManager != null)
        {
            _levelMusicManager.EndMusic();
        }
    }
}
