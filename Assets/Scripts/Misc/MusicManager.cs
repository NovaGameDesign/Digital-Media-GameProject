using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class MusicManager : MonoBehaviour
    {
        
        private AudioSource _audioSource;
        [SerializeField] AudioClip defaultSong;
        private AudioClip currentSong;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = defaultSong;
        }

        public void PlayMusic(AudioClip songToPlay)
        {
            if(songToPlay == currentSong) return;

            _audioSource.clip = songToPlay;
            _audioSource.Play();
            currentSong = songToPlay;
            
        }

        public void PlayDefaultSong()
        {
            _audioSource.clip = defaultSong;
            _audioSource.Play();
        }
        /*private void FixedUpdate()
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }*/
    }
}
