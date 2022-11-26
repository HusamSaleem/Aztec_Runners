using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource music;
    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        music = transform.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        music.volume = 1f;
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (music.isPlaying) return;
        music.Play();
        music.loop = true;
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void LowerVolume()
    {
        music.volume/=2.0f;
    }

    public void IncreaseVolume()
    {
        music.volume*=2.0f;
    }
}
