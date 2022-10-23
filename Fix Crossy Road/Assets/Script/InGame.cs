using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    public void PlaySFX()
    {
        SoundManager.Instance.asSFX.Play();
    }
}
