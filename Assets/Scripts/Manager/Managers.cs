using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent((typeof(InventoryManager)))]
[RequireComponent(typeof(WeatherManager))]
[RequireComponent(typeof(ImageManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(MissionManager))]
[RequireComponent(typeof(DataManager))]
public class Managers : GameEvent{
  public static PlayerManager Player { get; private set; }
  public static InventoryManager Inventory { get; private set; }
  public static WeatherManager Weather { get; private set; }
  public static NetworkService Network { get; private set; }
  public static ImageManager Image { get; private set; }
  public static AudioManager Audio { get; private set; }
  public static MissionManager Mission { get; private set; }
  public static DataManager Data { get; private set; }
  private List<IGameManager> startSequence;


  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  void Awake() {
    //сохраняем текущий объект между сценами
    DontDestroyOnLoad(gameObject);
    
    Network = new NetworkService();
    Player = GetComponent<PlayerManager>();
    Inventory = GetComponent<InventoryManager>();
    Weather = GetComponent<WeatherManager>();
    Image = GetComponent<ImageManager>();
    Audio = GetComponent<AudioManager>();
    Mission = GetComponent<MissionManager>();
    Data = GetComponent<DataManager>();
    startSequence = new List<IGameManager>();
    startSequence.Add(Player);
    startSequence.Add(Inventory);
    startSequence.Add(Weather);
    startSequence.Add(Image);
    startSequence.Add(Audio);
    startSequence.Add(Mission);
    startSequence.Add(Data);

    StartCoroutine(StartManagers());
    Debug.Log("Coroutine..");
  }

  private IEnumerator StartManagers() {
    foreach (IGameManager manager in startSequence) {
      manager.StartManager();
    }
    yield return null;
    int numModules = startSequence.Count;
    int numReady = 0;

    while (numReady < numModules) {
      int last = numReady;
      numReady = 0;
      foreach (IGameManager manager in startSequence) {
        if (manager.Status == ManagerStatus.Started) {
          numReady++;
        }
      }
      if (numReady > last) {
        Debug.Log("Progress: " + numReady + "/" + numModules);
        UpdLoadManager(numReady,numModules);
      }
      yield return null;
    }
    Debug.Log("All Managers started up");
    FinishedLoadManager();
  }

  
}