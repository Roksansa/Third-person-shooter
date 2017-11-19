using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager{
  [SerializeField] private AudioSource soundSource;
  [SerializeField] private AudioSource music1Source;
  [SerializeField] private AudioSource music2Source;
  [SerializeField] private string introBGMusic;

  [SerializeField] private string levelBGMusic;

  //плавный переход между мелодиями
  private AudioSource activeMusic;
  private AudioSource inactiveMusic;
  public float crossFadeRate = 1.5f;
  private bool crossFading;


  private float musicVolume;

  public float MusicVolume {
    get { return musicVolume; }
    set {
      musicVolume = value;
      if (music1Source != null && !crossFading) {
        music1Source.volume = musicVolume;
        music2Source.volume = musicVolume;
      }
    }
  }

  public bool MusicMute {
    get {
      if (music1Source != null) {
        return music1Source.mute;
      }
      return false;
    }
    set {
      if (music1Source != null) {
        music1Source.mute = value;
        music2Source.mute = value;
      }
    }
  }

  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  public ManagerStatus Status { get; private set; }

  public void StartManager() {
    Debug.Log("Audio manager starting...");

    SoundVolume = 1f;
    music1Source.ignoreListenerVolume = true;
    music1Source.ignoreListenerPause = true;
    music2Source.ignoreListenerVolume = true;
    music2Source.ignoreListenerPause = true;
    SoundVolume = 1f;
    MusicVolume = 1f;

    activeMusic = music1Source;
    inactiveMusic = music2Source;
    Status = ManagerStatus.Started;
  }

  public float SoundVolume {
    get { return AudioListener.volume; }
    set { AudioListener.volume = value; }
  }

  public bool SoundMute {
    get { return AudioListener.pause; }
    set { AudioListener.pause = value; }
  }

  public void PlaySound(AudioClip clip) {
//    soundSource.clip=clip; soundSource.Play();
    soundSource.PlayOneShot(clip);
  }

  public void PlayIntroMusic() {
    PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
  }

  public void PlayLevelMusic() {
    PlayMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
  }

  private void PlayMusic(AudioClip clip) {
    if (crossFading) {
      return;
    }
    StartCoroutine(CrossFadeMucis(clip));
  }

  private IEnumerator CrossFadeMucis(AudioClip clip) {
    crossFading = true;
    inactiveMusic.clip = clip;
    inactiveMusic.volume = 0;
    inactiveMusic.Play();
    float scaledRate = crossFadeRate*musicVolume;
    while (activeMusic.volume > 0) {
      if (activeMusic.clip != null && inactiveMusic.clip != null) {
        Debug.Log(activeMusic.clip.name + " "+ inactiveMusic.clip.name);
      }
      Debug.Log("I tutoch" + activeMusic.volume +  " " + inactiveMusic.volume);
      activeMusic.volume -= scaledRate*Time.deltaTime;
      inactiveMusic.volume += scaledRate*Time.deltaTime;
      yield return null;
    }
    AudioSource temp = activeMusic;
    activeMusic = inactiveMusic;
    activeMusic.volume = musicVolume;
    inactiveMusic = temp;
//    Debug.Log(activeMusic.clip.name + " "+ inactiveMusic.clip.name);
    inactiveMusic.Stop();
    crossFading = false;
  }

  public void StopMusic() {
    activeMusic.Stop();
    inactiveMusic.Stop();
    crossFading = false;
  }
}