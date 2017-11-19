using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkService{
  private const string imageApi = "pp.userapi.com/c635101/v635101107/2c0cd/AXb1es-6drY.jpg";

  private const string xmlApi =
    "http://api.openweathermap.org/data/2.5/weather?id=1496747&appid=03d5dc01df72a00e576d42d4c3226dc8&&mode=xml";

  private bool IsResponseValid(WWW www) {
    if (www.error != null) {
      Debug.Log("Bad connection");
      return false;
    }
    else if (string.IsNullOrEmpty(www.text)) {
      Debug.Log("Bad data");
      return false;
    }
    else {
      return true;
    }
  }

  private bool IsResponseValidImage(WWW www) {
    if (www.error != null) {
      Debug.Log("Bad connection");
      return false;
    }
    return true;
  }

  private IEnumerator CallAPI(string url, Action<string> callback) {
    WWW www = new WWW(url);
    yield return www; // while request ulr given UPRAVLENIE UNITYAPI

    if (!IsResponseValid(www))
      yield break;
    callback(www.text);
  }

  private IEnumerator CallAPI(string url, Action<Texture2D> callback) {
    WWW www = new WWW(url);
    Debug.Log("1");
    yield return www; // while request ulr given UPRAVLENIE UNITYAPI
    Debug.Log("2");
    if (!IsResponseValidImage(www))
      yield break;
    Debug.Log("3");
    callback(www.texture);
  }

  public IEnumerator GetWeatherXML(Action<string> callback) {
    return CallAPI(xmlApi, callback);
  }

  public IEnumerator DownloadImage(Action<Texture2D> callback) {
    return CallAPI(imageApi, callback);
  }
}