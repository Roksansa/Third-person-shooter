using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : GameEvent, IGameManager{
  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  public int Health { get; private set; }
  public int MaxHealth { get; private set; }
  public ManagerStatus Status { get; private set; }

  public void StartManager() {
    Debug.Log("Player manager starting...");
    Health = 50;
    MaxHealth = 100;
    Status = ManagerStatus.Started;
  }

  public void SetHealth(int value) {
    Health += value;
    if (Health > MaxHealth) {
      Health = MaxHealth;
    }
    else if (Health < 0) {
      Health = 0;
    }
    if (Health == 0) {
      FailedMission();
    }
    UpdateHealth();
    Debug.Log("Health: "+Health +"/"+MaxHealth);
  }
  
  public void Respawn() {
    Health = 50;
    MaxHealth = 100;
  }

  public void UpdateData(int health, int max) {
    Health = health;
    MaxHealth = max;
  }
}