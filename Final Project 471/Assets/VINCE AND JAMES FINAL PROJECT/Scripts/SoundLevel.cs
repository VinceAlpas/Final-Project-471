using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundLevel : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Image soundOnIcon;
    [SerializeField] Image soundOffIcon;
    private bool muted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetFloat("muted", 0);
            Load();
        }

        else
        {
            Load();
        }
        
        UpdateButtonIcon();
        AudioListener.pause = muted;

        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }

        else
        {
            Load();
        }
    }

    public void OnButtonPress()
    {
        if(muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }

        else{
            muted = false;
            AudioListener.pause = false;
        }

        Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if(muted == false)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }

        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");

    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
