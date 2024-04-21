using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum FloatType
{
    MasterVolume = 0,
    SfxVolume = 1,
    MusicVolume = 2
}

public class SerializeManager
{
    public static SerializeManager Instance { get; }

    static SerializeManager()
    {
        Instance = new SerializeManager();
    }

    private SerializeManager() { }

    public void SetFloat(FloatType type, float value) 
    { 
        PlayerPrefs.SetFloat(type.ToString(), value);
    }

    public float GetFloat(FloatType type) 
    { 
        if (PlayerPrefs.HasKey(type.ToString())) 
            return PlayerPrefs.GetFloat(type.ToString());
        else
        {
            PlayerPrefs.SetFloat(type.ToString(), 1f);
            return 1f;
        }
    }

    public void SetLevelLockedState(int levelSceneID, bool isLocked)
    {
        PlayerPrefs.SetInt("level_" + levelSceneID, Convert.ToInt32(isLocked));
    }

    public bool GetLevelLockedState(int levelSceneID)
    {
        if (PlayerPrefs.HasKey("level_" + levelSceneID))
            return Convert.ToBoolean(PlayerPrefs.GetInt("level_" + levelSceneID));
        else
        {
            SetLevelLockedState(levelSceneID, true);
            return true;
        }
    }
}
