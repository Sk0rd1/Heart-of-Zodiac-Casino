using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource buttonClickUI;
    [SerializeField]
    private AudioSource buttonSpinUI;
    [SerializeField]
    private AudioSource winSpin;
    [SerializeField]
    private AudioSource levelUp;
    [SerializeField]
    private AudioSource gameWin;
    [SerializeField]
    private AudioSource gameOver;
    [SerializeField]
    private AudioSource keyKnock;
    [SerializeField]
    private AudioSource keyMove;
    [SerializeField]
    private AudioSource pickUpCoin;

    [SerializeField]
    private AudioMixerGroup mixer;

    [SerializeField]
    private SwitchManager switchSoundOn;
    [SerializeField]
    private SwitchManager switchSoundOff;

    public void Start()
    {
        try
        {
            if (PlayerPrefs.GetInt("isSound") == 1)
            {
                switchSoundOn.gameObject.SetActive(true);
                switchSoundOff.gameObject.SetActive(false);
                mixer.audioMixer.SetFloat("MasterVolume", 0);
            }
            else
            {
                switchSoundOn.gameObject.SetActive(false);
                switchSoundOff.gameObject.SetActive(true);
                mixer.audioMixer.SetFloat("MasterVolume", -80);
            }
        }
        catch { }
    }

    public void ButtonClickUI()
    {
        buttonClickUI.Play();
    }

    public void ButtonSpinUI() 
    {
        buttonSpinUI.Play();
    }

    public void WinSpin()
    {
        winSpin.Play(); 
    }

    public void LevelUp()
    {
        levelUp.Play();
    }

    public void SetMasterVolume(float volume)
    {
        mixer.audioMixer.SetFloat("MasterVolume", volume);

        if (volume == 0f)
        {
            Debug.Log("Save 1");
        }
        else
        {
            Debug.Log("Save 0");
        }

        PlayerPrefs.Save();
    }

    public void GameOver()
    {
        gameOver.Play();
        keyMove.Stop();
    }

    public void GameWin()
    {
        gameWin.Play();
        keyMove.Stop();
    }

    public void KeyKnock()
    {
        keyKnock.Play();
    }

    public void KeyMove()
    {
        keyMove.Play();
    }

    public void KeyMoveStop()
    {
        keyMove.Stop();
    }

    public void PickUpCoin()
    {
        pickUpCoin.Play();
    }
}
