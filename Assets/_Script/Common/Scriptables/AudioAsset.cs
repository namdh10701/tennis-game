using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/AudioAsset", fileName = "AudioAsset")]
public class AudioAsset : ScriptableObject
{
    public AudioClip MatchSceneBGM;
    public AudioClip MenuSceneBGM;
    public AudioClip ButtonClick;
    public AudioClip BallHit;
    public AudioClip GameOver;
}