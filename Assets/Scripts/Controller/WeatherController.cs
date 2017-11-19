using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour{
  [SerializeField] private Material materialSky;
  [SerializeField] private Light sun;

  private float fullIntensity;
  public float Speed = 0.2f;
  private void Awake() {
    Debug.Log("Tut2 ");
    StartCoroutine(StartController());
  }

  private IEnumerator StartController() {
    while (Managers.Weather == null) {
      yield return null;
    }
    //include listeners
    Managers.Weather.UpdateWeather += SetOvercast;
    Debug.Log("Tut ");
  }

  private void OnDestroy() {
  //exclude listeners
    if (Managers.Weather != null) {
      Managers.Weather.UpdateWeather -= SetOvercast;
    }
  }

// Use this for initialization
  void Start() {
    fullIntensity = sun.intensity;
  }

// Update is called once per frame
  void Update() { }

  private void SetOvercast() {
    StartCoroutine(UpdateWeather());
  }

  private IEnumerator UpdateWeather() {
    float intensity = fullIntensity - (fullIntensity*Managers.Weather.CloudValue);
    while (sun.intensity > intensity) {
      sun.intensity = fullIntensity - (fullIntensity*Managers.Weather.CloudValue*Speed);
      materialSky.SetFloat("_Blend", Managers.Weather.CloudValue*Speed);
      Debug.Log(Speed + " "+ sun.intensity + " "+ Managers.Weather.CloudValue*Speed + "   "+ intensity);
      Speed += 0.2f;
      yield return new WaitForSeconds(.5f);
    }
  }
}