using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;


[System.Serializable]
public class Sound
{
    public string name = "";
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(-3f, 3f)]
    public float pitch = 1f;
    public bool loop = false;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [Space(12)]
    [Header("----- MUSIC -----")]
    public AudioSource asrc_sfx;
    public AudioSource mainMenuAudio;
    public AudioSource gamePlayAudio;


    public AudioClip level_1_Audio;
    public AudioClip level_2_Audio;

    [Space(12)]
    [Header("----- VFX ----")]
    public Sound[] sounds;


    public Image soundImage;
    public Sprite soundOff;
    public Sprite soundOn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }


        DontDestroyOnLoad(this);
    }

    public void Stop(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Equals(name))
            {
                asrc_sfx.Stop();
            }
        }
    }

    public void Play(string name)
    {
        bool soundFound = false;
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Equals(name))
            {
                asrc_sfx.PlayOneShot(sounds[i].clip);
                soundFound = true;
            }
        }
        if (!soundFound)
        {
            Debug.LogError(name + " SOUND CLIP DOESN't MATCH");
        }
    }


    public void OnClick_MusicButton()
    {
        if (asrc_sfx.volume==0)
        {
            asrc_sfx.volume = 1f;
            mainMenuAudio.volume = 1f;
            gamePlayAudio.volume = 1f;
            soundImage.sprite = soundOn;
        }
        else
        {
            asrc_sfx.volume = 0f;
            mainMenuAudio.volume = 0f;
            gamePlayAudio.volume = 0f;
            soundImage.sprite = soundOff;
        }
    }
}
