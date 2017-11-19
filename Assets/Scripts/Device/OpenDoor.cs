using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//функцию операте лучше реализовать через интерфейс, покрайней мере для инвенторя
//так как sendMessage работает через рефлексию - медленно в игре.
public class OpenDoor : BaseDevice{
  [SerializeField] private Vector3 dPos;

  private bool open;


  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() {
    if (open)
      base.Radius = 12;
    else
      base.Radius = 5;
    base.OnKeyClick();
  }

  public override void Operate() {
    if (open) {
      Vector3 pos = transform.position - dPos;
      transform.position = pos;
    }
    else {
      Vector3 pos = transform.position + dPos;
      transform.position = pos;
    }
    open = !open;
  }

  public void Activate() {
    if (!open) {
      Vector3 pos = transform.position + dPos;
      transform.position = pos;
      open = true;
    }
  }

  public void Deactivate() {
    if (open) {
      Vector3 pos = transform.position - dPos;
      transform.position = pos;
      open = false;
    }
  }
}