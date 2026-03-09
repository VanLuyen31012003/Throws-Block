using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource audioSource;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = this.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = audioClips[(int)ESoundIndex.Background];
        audioSource.Play();
    }
    public void PlaySoundButtonClick()
    {
        audioSource.PlayOneShot(audioClips[(int)ESoundIndex.ButtonClick]);
    }
    public void PlaySoundBlock()
    {
        audioSource.PlayOneShot(audioClips[(int)ESoundIndex.SoundBlock]);
    }

}
