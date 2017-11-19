using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class WanderingAI : MonoBehaviour{
  [SerializeField] private GameObject fireballPrefab;

  private GameObject fireball;
  private bool alive;
  public float ObstacleRange = 5.0f;
  public float Speed = 3.0f;

  void Start() {
    alive = true;
  }

  // Update is called once per frame
  void Update() {
    if (alive) {
      transform.Translate(0, 0, Speed*Time.deltaTime);
      Ray ray = new Ray(transform.position, transform.forward);
      RaycastHit hit;
      if (Physics.SphereCast(ray, 0.75f, out hit)) {
        GameObject hitObject = hit.transform.gameObject;
        if (hitObject.GetComponent<PlayerCharacter>()) {
          if (fireball == null) {
            fireball = Instantiate(fireballPrefab) as GameObject;
            fireball.transform.position =
              transform.TransformPoint(Vector3.forward*1.5f);
            fireball.transform.rotation = transform.rotation;
          }
        }
        else if (hit.distance < ObstacleRange) {
          float angle = Random.Range(-110, 110);
          transform.Rotate(0, angle, 0);
        }
      }
    }
  }

  public void SetAlive(bool alive) {
    alive = alive;
  }
}