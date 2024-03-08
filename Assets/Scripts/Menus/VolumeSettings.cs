using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider mouseSensivitySlider;

    public const string AUDIOMIXER_MASTER = "MasterVolume";
    public const string AUDIOMIXER_MUSIC = "MusicVolume";
    public const string AUDIOMIXER_SFX = "SFXVolume";

    private void Awake()
    {
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxVolumeSlider.value);
    }
    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(AUDIOMIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(AUDIOMIXER_SFX, Mathf.Log10(value) * 20);
    }
}
