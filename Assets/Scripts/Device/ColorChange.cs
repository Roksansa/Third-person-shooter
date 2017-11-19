using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : BaseDevice{
  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() {
    base.Radius = 3;
    base.OnKeyClick();
  }

  public override void Operate() {
    Color random = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    GetComponent<Renderer>().material.color = random;
    Managers.Image.GetTexture(SetTexture);
  }

  private void SetTexture(Texture2D tex) {
    GetComponent<Renderer>().material.mainTexture = tex;
  }
}