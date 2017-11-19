using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour, IGameManager{
  public ManagerStatus Status { get; private set; }

  private string filename;

  public void StartManager() {
    Debug.Log("Data manager starting...");
    filename = Path.Combine(
      //путь предоставленных юнити для хранения файлов
      //также в путь включаются данные из полей Company Name и Product Name, входящих в состав настроек 
      //PlayerSettings, поэтому, отредактировать при необходимости эти переменные
      Application.persistentDataPath, "game.dat");
    Status = ManagerStatus.Started;
  }

  public void SaveGameState() {
    Dictionary<string, object> gamestate = new Dictionary<string, object>(); //Словарь, который будет подвергнут сериализации.
    //записали
    gamestate.Add("Inventory", Managers.Inventory.GetData());
    gamestate.Add("Health", Managers.Player.Health);
    gamestate.Add("MaxHealth", Managers.Player.MaxHealth);
    gamestate.Add("CurLevel", Managers.Mission.CurLevel);
    gamestate.Add("MaxLevel", Managers.Mission.MaxLevel);
    gamestate.Add("OreCount", Managers.Mission.OreCount);
    FileStream stream = File.Create(filename); // Создаем файл по указанному адресу.
    BinaryFormatter formatter = new BinaryFormatter();
    //сериализовали
    formatter.Serialize(stream, gamestate);
    stream.Close();
  }

  public void LoadGameState() {
    if (!File.Exists(filename)) { // Переход к загрузке только при наличии файла.
      Debug.Log("No saved game");
      return;
    }
    Dictionary<string, object> gamestate; // Словарь для размещения загруженных данных.
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream stream = File.Open(filename, FileMode.Open);
    //вытянули
    gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
    stream.Close();
    Managers.Inventory.UpdateData((Dictionary<string, int>) gamestate["Inventory"]); // Обновляем диспетчеры, снабжая их десериализованными данными.
    Managers.Player.UpdateData((int) gamestate["Health"], (int) gamestate["MaxHealth"]);
    Managers.Mission.UpdateData((int) gamestate["CurLevel"],(int) gamestate["MaxLevel"], (int) gamestate["OreCount"]);
    Managers.Mission.RestartCurrent();

  }
}