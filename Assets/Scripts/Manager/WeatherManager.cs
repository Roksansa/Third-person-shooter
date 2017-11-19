using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
public class WeatherManager : MonoBehaviour, IGameManager{
  private NetworkService network;

  public float CloudValue { get; private set; }
  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  public ManagerStatus Status { get; private set; }

  public void StartManager() {
    Debug.Log("Weather manager starting...");
    network = Managers.Network;
    Debug.Log(network);
    if (network != null) {
      StartCoroutine(network.GetWeatherXML(OnXMLDataLoaded));
    }
    Status = ManagerStatus.Initializing;
  }

  private void OnXMLDataLoaded(string obj) {
    Debug.Log(obj);
    XmlDocument doc = new XmlDocument();
    doc.LoadXml(obj);
    XmlNode root = doc.DocumentElement;
    XmlNode node = root.SelectSingleNode("clouds");
    string value = node.Attributes["value"].Value;
    CloudValue = Convert.ToInt32(value)/100f;
    Debug.Log("Value: "+ CloudValue);
    //рассылка для остальных сценарией реализовать через делегаты
    UpdateWeather();
    Status = ManagerStatus.Started;
  }
  
  public delegate void OnWeatherUpdated();
  public event OnWeatherUpdated UpdateWeather = delegate {  };
}