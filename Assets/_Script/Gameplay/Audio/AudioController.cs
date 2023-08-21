using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using DG.Tweening;

namespace Audio
{
    public class AudioController : SingletonPersistent<AudioController>
    {
        public AudioAsset AudioAsset;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;

        private bool _isMusicOn;
        private bool _isSoundOn;

        public void Init(bool isMusicOn, bool isSoundOn)
        {
            _isMusicOn = isMusicOn;
            _isSoundOn = isSoundOn;
            _musicSource.volume = !_isMusicOn ? 0 : .33f;
            _soundSource.volume = !_isSoundOn ? 0 : .6f;
        }
        public void ToggleSound(bool isSoundOn)
        {
            _isSoundOn = isSoundOn;
            _soundSource.DOFade(!_isSoundOn ? 0 : .6f, .3f);
        }

        public void ToggleMusic(bool isMusicOn)
        {
            _isMusicOn = isMusicOn;
            _musicSource.DOFade(!_isMusicOn ? 0 : .33f, .3f);
        }

        protected override void Awake()
        {
            base.Awake();
            _musicSource.volume = .33f;
            _soundSource.volume = .6f;
        }
        public void PlaySound(AudioClip clip)
        {
            _soundSource.PlayOneShot(clip);
        }
        public void PlaySound(string clip)
        {
            switch (clip)
            {
                case "button":
                    _soundSource.PlayOneShot(AudioAsset.ButtonClick);
                    break;
                case "ball_hit":
                    _soundSource.PlayOneShot(AudioAsset.BallHit, 1f);
                    break;
                case "game_over":
                    _soundSource.PlayOneShot(AudioAsset.GameOver);
                    break;
            }
        }
        public void CrossfadeMusic(AudioClip newClip, float fadeDuration)
        {
            StartCoroutine(CrossfadeMusicCoroutine(newClip, fadeDuration));
        }

        private IEnumerator CrossfadeMusicCoroutine(AudioClip newClip, float fadeDuration)
        {
            float startVolume = _musicSource.volume;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }

            _musicSource.Stop();
            _musicSource.clip = newClip;
            _musicSource.Play();

            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
                yield return null;
            }
        }
    }
}
