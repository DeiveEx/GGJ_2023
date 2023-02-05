using UnityEngine;

[CreateAssetMenu(fileName = "Sound Cue", menuName = "Custom/New Sound Cue")]
public class SoundCue : ScriptableObject
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private SoundMode _mode;
    [SerializeField] private bool _loop;
    
    [Header("Only use these if \"Mode\" is set to \"EffectAtRandomTimes\"")]
    [SerializeField] private float _effectRandomPlayTimeMin;
    [SerializeField] private float _effectRandomPlayTimeMax;

    public AudioClip AudioClip => _audioClip;
    public SoundMode Mode => _mode;
    public bool Loop => _loop;
    public float EffectRandomPlayTimeMin => _effectRandomPlayTimeMin;
    public float EffectRandomPlayTimeMax => _effectRandomPlayTimeMax;
}
