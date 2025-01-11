using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private String[] soundGroups;
    private int selectedPixelSize = 100;

    private void Start() {
        foreach(string group in soundGroups) {
            if (PlayerPrefs.HasKey(group)) {
                LoadVolume(group);
            } else {
                SetVolume(1, group);
            }
        }
    }
    
    public void SetVolume(float volume, string group) {
        mainAudioMixer.SetFloat(group, Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat(group, volume);
    }

    private void LoadVolume(string group) {
        float volume = PlayerPrefs.GetFloat(group);
        SetVolume(volume, group);
    }
}

