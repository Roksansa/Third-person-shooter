using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour{
  [SerializeField] private SettingsPopup popup;
  [SerializeField] private InventoryPopup inventoryPopup;
  [SerializeField] private Text healthLabel;
  [SerializeField] private Text levelEnding;
  [SerializeField] private Text CameraModeHelpText;
  [SerializeField] private MainMenuPopup mainMenuPopup;
  public static bool PauseGame = false;

  private void Awake() {
    StartCoroutine(StartController());
  }

  private IEnumerator StartController() {
    while (Managers.Player == null || Managers.Mission == null) {
      yield return null;
    }
    //include listeners
    Managers.Player.UpdateEvent += SetHealth;
    Managers.Mission.MissionComplete += LevelComplete;
    Managers.Player.MissionFailed += LevelFailed;
    Managers.Mission.GameComplete += AllLevelComplete;
    Managers.Mission.MissionGoal += UIShowGoal;
    Debug.Log("Tut ");
  }

  private void AllLevelComplete() {
    levelEnding.gameObject.SetActive(true);
    levelEnding.text = "You Finished the Game!";
  }

  private void SetHealth() {
    string message = "Health: " + Managers.Player.Health + "/" + Managers.Player.MaxHealth;
    healthLabel.text = message;
    healthLabel.enabled = true;
  }

  private void OnDestroy() {
    //exclude listeners
    if (Managers.Player != null) {
      Managers.Player.UpdateEvent -= SetHealth;
      Managers.Player.MissionFailed -= LevelFailed;
    }
    if (Managers.Mission != null) {
      Managers.Mission.MissionComplete -= LevelComplete;
      Managers.Mission.GameComplete -= AllLevelComplete;
      Managers.Mission.MissionGoal -= UIShowGoal;
    }
  }

  void Start() {
    PauseGame = false;
    SetHealth();
    popup.gameObject.SetActive(false);
    inventoryPopup.gameObject.SetActive(false);
    levelEnding.gameObject.SetActive(false);
    mainMenuPopup.gameObject.SetActive(false);
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.M)) {
      bool isShowing = !popup.gameObject.activeSelf;
      if (isShowing) {
        if (inventoryPopup.gameObject.activeSelf) {
          inventoryPopup.gameObject.SetActive(!isShowing);
        }
        if (mainMenuPopup.gameObject.activeSelf) {
          mainMenuPopup.gameObject.SetActive(!isShowing);
        }
        Time.timeScale = 0f;
        PauseGame = true;
      }
      else {
        Time.timeScale = 1f;
        PauseGame = false;
      }
      popup.gameObject.SetActive(isShowing);
    }
    if (Input.GetKeyDown(KeyCode.I)) {
      bool isShowing = !inventoryPopup.gameObject.activeSelf;
      if (isShowing) {
        if (popup.gameObject.activeSelf) {
          popup.gameObject.SetActive(!isShowing);
        }
        if (mainMenuPopup.gameObject.activeSelf) {
          mainMenuPopup.gameObject.SetActive(!isShowing);
        }
        inventoryPopup.Refresh();
        Time.timeScale = 0f;
        PauseGame = true;
      }
      else {
        Time.timeScale = 1f;
        PauseGame = false;
      }
      inventoryPopup.gameObject.SetActive(isShowing);
    }
    if (Input.GetKeyDown(KeyCode.P)) {
      bool isShowing = !mainMenuPopup.gameObject.activeSelf;
      if (isShowing) {
        if (inventoryPopup.gameObject.activeSelf) {
          inventoryPopup.gameObject.SetActive(!isShowing);
        }
        if (popup.gameObject.activeSelf) {
          popup.gameObject.SetActive(!isShowing);
        }
        Time.timeScale = 0f;
        PauseGame = true;
      }
      else {
        Time.timeScale = 1f;
        PauseGame = false;
      }
      mainMenuPopup.gameObject.SetActive(isShowing);
    }
    if (TPSCamera.Mode2D) {
      CameraModeHelpText.text = "You can jump only in 3D mode. \n" +
                                "Press \"Q\" to switch to 3D mode";
    }
    else if (!TPSCamera.Mode2D) {
      CameraModeHelpText.text = "Press \"Q\" to switch to 2D mode";
    }
  }

  private void LevelComplete() {
    StartCoroutine(CompleteLevel());
  }

  private IEnumerator CompleteLevel() {
    levelEnding.gameObject.SetActive(true);
    levelEnding.text = "Level Complete!";
    yield return new WaitForSeconds(2);
    Managers.Mission.GoToNext();
  }

  private void LevelFailed() {
    StartCoroutine(FailLevel());
  }

  private IEnumerator FailLevel() {
    levelEnding.gameObject.SetActive(true);
    levelEnding.text = "Level Failed";
    yield return new WaitForSeconds(2);
    Managers.Player.Respawn();
    Managers.Mission.RestartCurrent();
  }

  public void UIShowGoal() {
    StartCoroutine(UIShowGoalText());
  }

  private IEnumerator UIShowGoalText() {
    levelEnding.gameObject.SetActive(true);
    levelEnding.text = "You have to collect "+Managers.Mission.OreCount + " Ores to pass the level";
    yield return new WaitForSeconds(2);
    levelEnding.gameObject.SetActive(false);
  }


}