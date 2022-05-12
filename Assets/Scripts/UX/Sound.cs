using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string _name;//name of sound 
    public AudioMixerGroup _group;//mixer group
    public AudioClip _clip;//audio clip

    [Range(0f, 1f)]
    public float _volume = 1f;//volume value

    [Range(0.1f, 3f)]
    public float _pitch = 1f;//pitch value
    //source value
    [HideInInspector]
    public AudioSource _source;
    //should it be looped 
    public bool _loop;
    public bool _ignoreAudioListenerPause;
}
