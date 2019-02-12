using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialog/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField] Sprite _sprite;
    [SerializeField] Font _font;
    [SerializeField] AudioClip _voiceAudio;

    public Sprite Sprite { get { return _sprite; } private set { _sprite = value; } }
    public Font Font { get { return _font; } private set { _font = value; } }
    public AudioClip VoiceAudio { get { return _voiceAudio; } private set { _voiceAudio = value; } }
}
