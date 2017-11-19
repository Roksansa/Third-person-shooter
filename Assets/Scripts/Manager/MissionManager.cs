using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : GameEvent, IGameManager{
  public ManagerStatus Status { get; private set; }
  public int CurLevel { get; private set; }
  public int MaxLevel { get; private set; }
  public int OreCount { get; private set; }
  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }


  public void StartManager() {
    Debug.Log("Mission manager starting...");
    UpdateData(0, 2, 3);
    Status = ManagerStatus.Started;
  }

  public void GoToNext() {
    if (CurLevel < MaxLevel) {
      CurLevel++;
      string name = "Level" + CurLevel;
      Debug.Log("Loading " + name);
      Managers.Inventory.Restart("new");
      SceneManager.LoadScene(name);
    }
    else {
      Debug.Log("Last level");
      CompleteGame();
    }
  }

  public void MissionCompleted() {
    if (OreCount <= Managers.Inventory.GetItemsCount("Ore")) {
      FinishMission();
    }
    else {
      ShowGoal();
    }
  }

  public void RestartCurrent() {
    string name = "Level" + CurLevel;
    Debug.Log("Loading " + name);
    Managers.Inventory.Restart("cur");
    SceneManager.LoadScene(name);
  }

  public void UpdateData(int cur, int max, int count) {
    CurLevel = cur;
    MaxLevel = max;
    OreCount = count;
  }
  
}