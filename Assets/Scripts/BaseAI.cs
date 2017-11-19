using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour{
  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  void OnGUI() {
    int posX = 10;
    int posY = 10;
    int width = 100;
    int height = 30;
    int buffer = 10;
    List<string> itemList = Managers.Inventory.GetItemsList();
    if (itemList.Count == 0) {
      GUI.Box(new Rect(posX, posY, width, height), "No Items");
    }
    foreach (string item in itemList) {
      int count = Managers.Inventory.GetItemsCount(item);
      Texture2D image = Resources.Load<Texture2D>("Icons/" + item);
      GUI.Box(new Rect(posX, posY, width, height),
        new GUIContent("(" + count + ")", image));
      posX += width + buffer;
    }
    string equipped = Managers.Inventory.EquippedItem;
    if (equipped != null) {
      posX = Screen.width - (width + buffer);
      Texture2D image = Resources.Load<Texture2D>("Icons/" + equipped);
      GUI.Box(new Rect(posX, posY, width, height), new GUIContent("Equipped ", image));
    }
    posX = 10;
    posY += height + buffer;
    foreach (string item in itemList) {
      if (item == "Health") {
        if (GUI.Button(new Rect(posX, posY, width, height), "Use Health")) {
          Managers.Inventory.ConsumeItem("Health");
          Managers.Player.SetHealth(25);
        }
      }
      else if (item == "Key"){
        if (GUI.Button(new Rect(posX, posY, width, height), "Equip " + item)) {
          Managers.Inventory.EquipItem(item);
        }
      }
      posX += width + buffer;
    }
    posY += height + buffer;
    posX = 10;
    GUI.Box(new Rect(posX, posY, width, height), Managers.Player.Health + "/"+Managers.Player.MaxHealth);
  }
}