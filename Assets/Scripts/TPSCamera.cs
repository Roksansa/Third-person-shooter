using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour{
  [SerializeField] private Transform target;
  private static bool mode2D = false;

  public static bool Mode2D {
    get { return mode2D; }
    private set { mode2D = value; }
  }

  public float rotSpeed = 1.5f;

  private float rotV;
  private Vector3 offset;

  // Use this for initialization
  void Start() {
    rotV = transform.eulerAngles.y;
    offset = target.position - transform.position;
  }

  void Update() {
    if (!UIController.PauseGame) {
      if (Input.GetKeyDown(KeyCode.Q)) {
        mode2D = !mode2D;
      }
    }
  }

  // Update is called once per frame
  void LateUpdate() {
    if (!UIController.PauseGame) {
      if (mode2D) {
        rotV -= Input.GetAxis("Horizontal")*rotSpeed;
        Quaternion rotation = Quaternion.Euler(0, rotV, 0);
        transform.position = target.position - (rotation*offset);
        if (transform.position.y < 10) {
          transform.position = new Vector3(transform.position.x, transform.position.y + 15, transform.position.z + 5);
        }
//      Debug.Log(transform.position.x+" "+transform.position.y + " "+ transform.position.z);
      }
      else {
        float horInput = Input.GetAxis("Horizontal");
        if (horInput != 0) {
          rotV += horInput*rotSpeed;
        }
        else {
          rotV += Input.GetAxis("Mouse X")*rotSpeed*3;
        }
        Quaternion rot = Quaternion.Euler(0, rotV, 0);
        transform.position = target.position - rot*offset;
      }
      transform.LookAt(target);
    }
  }
}