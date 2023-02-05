using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] private SoundMode _mode;
    [SerializeField] private Slider _slider;

    private void OnEnable()
    {
        float currentVolume = GlobalManager.Instance.SoundManager.GetVolume(_mode);
        _slider.SetValueWithoutNotify(currentVolume);
    }

    public void SetVolume(float value)
    {
        GlobalManager.Instance.SoundManager.SetVolume(_slider.value, _mode);
    }
}
