using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour{
  [SerializeField] private AudioClip clip;

  [SerializeField] private Slider slider;
  [SerializeField] private Slider sliderMusic;
  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  public void OnSoundToggle() {
    Managers.Audio.SoundMute = !Managers.Audio.SoundMute;
    Managers.Audio.PlaySound(clip);
    slider.interactable = !slider.interactable;
  }

  public void OnSoundValue(float volume) {
    Managers.Audio.SoundVolume = volume;
  }

  public void OnPlayMusic(int selector) {
    Managers.Audio.PlaySound(clip);
    switch (selector) {
      case 1:
        Managers.Audio.PlayIntroMusic();
        break;
      case 2:
        Managers.Audio.PlayLevelMusic();
        break;
      default:
        Managers.Audio.StopMusic();
        break;
    }
  }
  
  public void OnMusicToggle() {
    Managers.Audio.MusicMute = !Managers.Audio.MusicMute;
    Managers.Audio.PlaySound(clip);
    sliderMusic.interactable = !sliderMusic.interactable;
  }
  public void OnMusicValue(float volume) {
    Managers.Audio.MusicVolume = volume;
  }
}