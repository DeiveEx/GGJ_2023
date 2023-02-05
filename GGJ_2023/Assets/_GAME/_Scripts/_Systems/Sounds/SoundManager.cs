using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public enum SoundMode
{
    None,
    Music,
    Ambient,
    SoundEffect,
    EffectAtRandomTimes
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _ambientGroup;
    [SerializeField] private AudioMixerGroup _soundEffectsGroup;
    [SerializeField] private AudioSource _sourcePrefab;
    [SerializeField] private Vector2 _minMaxEffectPitch;
    [SerializeField] private Vector2 _minMaxEffectVolume;

    private List<AudioSource> _sources = new();
    private Dictionary<SoundCue, AudioSource> _loopingSounds = new();

    public void PlaySound(SoundCue sound)
    {
        if (sound.AudioClip == null)
        {
            Debug.LogWarning($"No sound clip defined", sound);
            return;
        }

        if (sound.Mode == SoundMode.EffectAtRandomTimes)
        {
            StartCoroutine(PlayAtRandomTimes(sound));
            return;
        }
        
        var source = GetSource();
        source.loop = sound.Loop;
        
        PlaySoundCue(sound, source);

        if (sound.Loop)
            _loopingSounds.Add(sound, source);
    }

    public void StopSound(SoundCue sound)
    {
        if (_loopingSounds.TryGetValue(sound, out var source))
        {
            source.Stop();
            source.clip = null;
            _loopingSounds.Remove(sound);
        }
    }

    public void SetVolume(float normalizedVolume, SoundMode mode = SoundMode.None)
    {
        _mixer.SetFloat(GetVolumeKey(mode), Mathf.Lerp(-80, 20, normalizedVolume));
    }

    public float GetVolume(SoundMode mode = SoundMode.None)
    {
        if(_mixer.GetFloat(GetVolumeKey(mode), out var volume))
        {
            return Mathf.InverseLerp(-80, 20, volume);
        }

        Debug.LogError($"Failed to get volume for [{GetVolumeKey(mode)}]");
        return 0;
    }

    private AudioMixerGroup GetGroup(SoundMode mode)
    {
        switch (mode)
        {
            case SoundMode.Music:
                return _musicGroup;
            
            case SoundMode.Ambient:
                return _ambientGroup;
            
            case SoundMode.SoundEffect:
            case SoundMode.EffectAtRandomTimes:
                return _soundEffectsGroup;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    private string GetVolumeKey(SoundMode mode)
    {
        switch (mode)
        {
            case SoundMode.None:
                return "MasterVolume";
            
            case SoundMode.Music:
                return "MusicVolume";
            
            case SoundMode.Ambient:
                return "AmbientVolume";
            
            case SoundMode.SoundEffect:
            case SoundMode.EffectAtRandomTimes:
                return "SoundEffectsVolume";
            
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    private void PlaySoundCue(SoundCue sound, AudioSource source)
    {
        source.outputAudioMixerGroup = GetGroup(sound.Mode);
        source.clip = sound.AudioClip;

        if (sound.Mode == SoundMode.SoundEffect)
        {
            source.pitch = Random.Range(_minMaxEffectPitch.x, _minMaxEffectPitch.y);
            source.volume = Random.Range(_minMaxEffectVolume.x, _minMaxEffectVolume.y);
        }
        else
        {
            source.pitch = 1;
            source.volume = 1;
        }
        
        source.Play();
    }

    private AudioSource GetSource()
    {
        foreach (var source in _sources)
        {
            if (!source.isPlaying)
                return source;
        }

        var newSource = Instantiate(_sourcePrefab, transform);
        _sources.Add(newSource);
        return newSource;
    }

    private IEnumerator PlayAtRandomTimes(SoundCue sound)
    {
        var source = GetSource();
        source.clip = sound.AudioClip;
        
        _loopingSounds.Add(sound, source);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(sound.EffectRandomPlayTimeMin, sound.EffectRandomPlayTimeMax));
            
            if(!_loopingSounds.ContainsKey(sound))
                break;
            
            PlaySoundCue(sound, source);
        }
    }
}
