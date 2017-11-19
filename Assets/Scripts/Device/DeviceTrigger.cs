using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour{
  [SerializeField] private GameObject[] targets;

  public bool requireKey;

  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  void OnTriggerEnter(Collider other) {
    if (requireKey && Managers.Inventory.EquippedItem != "Key") {
      return;
    }
    foreach (GameObject target in targets) {
      OpenDoor oper = target.GetComponent<OpenDoor>();
      if (oper != null) {
        oper.Activate();
      }
    }
  }

  void OnTriggerExit(Collider other) {
    foreach (GameObject target in targets) {
      OpenDoor oper = target.GetComponent<OpenDoor>();
      if (oper != null) {
        oper.Deactivate();
      }
    }
  }
  
}