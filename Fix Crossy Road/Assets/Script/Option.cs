using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] Button btnBGM;
    [SerializeField] Button btnSFX;
    public Sprite imgSoundOn;
    public Sprite imgSoundOff;

    public void Start()
    {
        if (SoundManager.Instance.asMusic.mute == true)
        {
            btnBGM.image.sprite = imgSoundOff;
        }
        else
        {
            btnBGM.image.sprite = imgSoundOn;
        }

        if (SoundManager.Instance.asSFX.mute == true && SoundManager.Instance.asSFXJump.mute == true && SoundManager.Instance.asSFXDie.mute == true)
        {
            btnSFX.image.sprite = imgSoundOff;
        }
        else
        {
            btnSFX.image.sprite = imgSoundOn;
        }
    }

    public void ButtonBGM()
    {
        SoundManager.Instance.MuteBGM();

        if (SoundManager.Instance.asMusic.mute == true)
        {
            btnBGM.image.sprite = imgSoundOff;
        }
        else
        {
            btnBGM.image.sprite = imgSoundOn;
        }
    }

    public void ButtonSFX()
    {
        SoundManager.Instance.MuteSFX();
        if (SoundManager.Instance.asSFX.mute == true && SoundManager.Instance.asSFXJump.mute == true && SoundManager.Instance.asSFXDie.mute == true)
        {
            btnSFX.image.sprite = imgSoundOff;
        }
        else
        {
            btnSFX.image.sprite = imgSoundOn;
        }
    }

    public void PlaySFX()
    {
        SoundManager.Instance.asSFX.Play();
    }
}
