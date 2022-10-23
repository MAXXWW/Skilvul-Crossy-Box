using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource asMusic;
    public AudioSource asSFX;
    public AudioSource asSFXJump;
    public AudioSource asSFXDie;

    private void Awake()
    {
        asSFX = transform.GetChild(0).GetComponent<AudioSource>();
        asMusic = transform.GetChild(1).GetComponent<AudioSource>();
        asSFXJump = transform.GetChild(2).GetComponent<AudioSource>();
        asSFXDie = transform.GetChild(3).GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MuteBGM()
    {
        if (asMusic.mute == false)
        {
            asMusic.mute = true;
        }
        else
        {
            asMusic.mute = false;
        }
    }

    public void MuteSFX()
    {
        if (asSFX.mute == false && asSFXJump.mute == false && asSFXDie.mute == false)
        {
            asSFX.mute = true;
            asSFXJump.mute = true;
            asSFXDie.mute = true;
        }
        else
        {
            asSFX.mute = false;
            asSFXJump.mute = false;
            asSFXDie.mute = false;
        }
    }
}
