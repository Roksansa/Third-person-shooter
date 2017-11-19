using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour, IGameManager{
  public ManagerStatus Status { get; private set; }
  private NetworkService network;
  private Texture2D texture2D;
  public void StartManager() {
    Debug.Log("Image manager starting...");
    network = Managers.Network;
    Debug.Log(network);
    Status = ManagerStatus.Started;
  }

  public void GetTexture(Action<Texture2D> callback) {
    if (texture2D == null) {
      Debug.Log("i tut");
      Debug.Log("5");
      StartCoroutine(network.DownloadImage((Texture2D image) => {
        Debug.Log("6");
        texture2D = image;
        callback(texture2D);
      }));
    }
    Debug.Log("7");
    Debug.Log("i tut11111");
    callback(texture2D);
  }
  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }
}