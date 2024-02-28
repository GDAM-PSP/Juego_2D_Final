using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FinalMusic : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource music;
    public float delay;

    void Start()
    {
        music = GetComponent<AudioSource>();
        StartCoroutine(PlayDelayedSoundCoroutine());
        music.mute = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator  PlayDelayedSoundCoroutine()
    {
        yield return new WaitForSeconds(delay);
        if(soundClip != null )
        {
            music.clip = soundClip;
            music.Play();
        }
      
    }
}
