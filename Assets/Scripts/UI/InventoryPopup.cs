using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour{
  [SerializeField] private Image[] itemIcons;
  [SerializeField] private Text[] itemLabels;
  [SerializeField] private Text curItemLabel;
  [SerializeField] private Button equipButton;

  [SerializeField] private Button useButton;

  // Use this for initialization
  private string curItem;

  void Start() { }

  // Update is called once per frame
  void Update() { }

  public void Refresh() {
    List<string> itemList = Managers.Inventory.GetItemsList();
    int len = itemIcons.Length;
    for (int i = 0; i < len; i++) {
      if (i < itemList.Count) {
        itemIcons[i].gameObject.SetActive(true);
        itemLabels[i].gameObject.SetActive(true);
        string item = itemList[i];
        Sprite sprite = Resources.Load<Sprite>("Icons/" + item);
        itemIcons[i].sprite = sprite;
// itemIcons[i].SetNativeSize(); //Изменение размеров изображения под исходный размер спрайта.
        int count = Managers.Inventory.GetItemsCount(item);
        string message = "x" + count;
        itemLabels[i].fontSize = 50;
        if (item == Managers.Inventory.EquippedItem) {
          message = "Equipped\n" + message;
          itemLabels[i].fontSize = 25;
        }
        itemLabels[i].text = message;
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((BaseEventData data) => {
          OnItem(item); //Лямбда-функция, позволяющая по-разному активировать каждый элемент.
        });
        EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
        trigger.triggers.Clear();
        trigger.triggers.Add(entry);
      }
      else {
        itemIcons[i].gameObject.SetActive(false); 
        itemLabels[i].gameObject.SetActive(false); 
      }
    }
    if (!itemList.Contains(curItem)) {
      curItem = null;
    }
    if (curItem == null) { //Скрываем кнопки при отсутствии выделенных элементов.
      curItemLabel.gameObject.SetActive(false);
      equipButton.gameObject.SetActive(false);
      useButton.gameObject.SetActive(false);
    }

    else { //Отображение выделенного в данный момент элемента.
      curItemLabel.gameObject.SetActive(true);
      equipButton.gameObject.SetActive(true);
      if (curItem == "Health") {
        useButton.gameObject.SetActive(true);
      }
      else {
        useButton.gameObject.SetActive(false);
      }
      curItemLabel.text = curItem + ":";
    }
  }

  public void OnItem(string item) { // Функция, вызываемая подписчиком события щелчка мыши.
    curItem = item;
    Refresh(); //Актуализируем отображение инвентаря после внесения изменений.
  }

  public void OnEquip() {
    Managers.Inventory.EquipItem(curItem);
    Refresh();
  }

  public void OnUse() {
    Managers.Inventory.ConsumeItem(curItem);
    if (curItem == "Health") {
      Managers.Player.SetHealth(25);
    }
    Refresh();
  }
}