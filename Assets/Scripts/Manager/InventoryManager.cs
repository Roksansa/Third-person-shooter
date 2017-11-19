using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager{
  // Use this for initialization
  public ManagerStatus Status { get; private set; }

  private Dictionary<string, int> items;
  public string EquippedItem { get; private set; }

  void Start() { }

  // Update is called once per frame
  void Update() { }

  void IGameManager.StartManager() {
    Debug.Log("Inventory manager starting...");
    items = new Dictionary<string, int>();
//    UpdateData(new Dictionary<string, int>());
    Status = ManagerStatus.Started;
  }

  private void ShowItems() {
    StringBuilder strnew = new StringBuilder("Item: ");
    foreach (KeyValuePair<string, int> str in items) {
      strnew.Append(str.Key + ":" + str.Value + " ");
    }
    Debug.Log(strnew.ToString());
  }

  public void AddItem(string name) {
    if (items.ContainsKey(name)) {
      items[name] += 1;
    }
    else {
      items[name] = 1;
    }
    ShowItems();
  }

  public List<string> GetItemsList() {
    List<string> list = new List<string>(items.Keys);
    return list;
  }

  public int GetItemsCount(string name) {
    if (items.ContainsKey(name)) {
      return items[name];
    }
    return 0;
  }

  public bool EquipItem(string name) {
    if (items.ContainsKey(name) && EquippedItem != name) {
      EquippedItem = name;
      Debug.Log("Equipped "+name);
      return true;
    }
    EquippedItem = null;
    Debug.Log("Unequipped"); 
    return false;
  }

  public bool ConsumeItem(string name) {
    if (items.ContainsKey(name)) {
      items[name]--;
      if (items[name] == 0) {
        items.Remove(name);
        if (EquippedItem == name) {
          EquippedItem = null;
        }
      }
    }
    else {
      Debug.Log("cannot consume "+ name);
      return false;
    }
    ShowItems();
    return true;
  }
  
  public void UpdateData(Dictionary<string, int> items) {
    this.items = items;
  }
  public Dictionary<string, int> GetData() { 
    return items;
  }

  public void Restart(string str) {
    if (str == "new") {
      Dictionary<string, int> itemsNew = new Dictionary<string, int>();
      Debug.Log("Update inventory");
      string name = "Health";
      while (ConsumeItem(name)) {
        if (itemsNew.ContainsKey(name)) {
          itemsNew[name] += 1;
        }
        else {
          itemsNew[name] = 1;
        }
      }
      items = itemsNew;
    }
    else {
      items = new Dictionary<string, int>();
    }
  }
}