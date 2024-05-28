using System;
using UnityEngine;


[Serializable]
public class GameSetting
{
    public static string saveString ="GameSetting.json";
    public SoundSetting soundSetting;
}

[Serializable]
public class SoundSetting
{
    [Range(0.0f,1.0f)]
    public float mainVolume;
    [Range(0.0f,1.0f)]
    public float bgVolume;
    [Range(0.0f,1.0f)]
    public float soundVolume;
}