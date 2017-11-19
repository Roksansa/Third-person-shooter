using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent: MonoBehaviour{
  public delegate void OnEventHandler();
  public delegate void OnLoadManagerEventHandler(int a, int b);
  
  public event OnEventHandler UpdateEvent = delegate {  };

  public event OnLoadManagerEventHandler UpdateLoadManager = delegate {  };
  
  public event OnEventHandler FinishesLoad = delegate {  };

  public event OnEventHandler MissionComplete = delegate {  };
  
  public event OnEventHandler MissionFailed = delegate {  };
  
  public event OnEventHandler GameComplete = delegate {  };
  
  public event OnEventHandler MissionGoal = delegate {  };
  
  protected void UpdateHealth() {
    UpdateEvent();
  }
  protected void UpdLoadManager(int a, int b) {
    UpdateLoadManager(a,b);
  }
  protected void FinishedLoadManager() {
    FinishesLoad();
  }

  protected void FinishMission() {
    MissionComplete();
  }

  protected void FailedMission() {
    MissionFailed();
  }

  protected void CompleteGame() {
    GameComplete();
  }
  
  protected void ShowGoal() {
    MissionGoal();
  }
}