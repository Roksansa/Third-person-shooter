using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseDevice : MonoBehaviour{
  private float radius = 2.5f;

  public float Radius {
    get { return radius; }
    set { radius = value; }
  }

  private GameObject playerObject;

  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  protected void OnKeyClick() {
    if (!TPSCamera.Mode2D && Input.GetButtonDown("Fire3")) {
      if (playerObject == null) {
        playerObject = GameObject.FindWithTag("Player");
      }

      Transform player = GameObject.FindWithTag("Player").transform;
      Debug.Log(player.position + "    " + transform.position);
      if (Vector3.Distance(player.position, transform.position) < radius) {
        Vector3 direction = transform.position - player.position;
        if (Vector3.Dot(player.forward, direction) > .5f) {
          Operate();
        }
      }
    }
  }

  void OnMouseDown() {
    if (TPSCamera.Mode2D) {
      //поверим наслово что такой объект есть
      Transform player = GameObject.FindWithTag("Player").transform;
      if (Vector3.Distance(player.position, transform.position) < radius) {
        Vector3 direction = transform.position - player.position;
        if (Vector3.Dot(player.forward, direction) > .5f) {
          Operate();
        }
      }
    }
  }

//  public virtual void OperateOnMouseClick() {
//    // поведение конкретного устройства
//  }

  public virtual void Operate() {
    // поведение конкретного устройства
  }
}