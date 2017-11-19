using UnityEngine;

public class CollectItem : MonoBehaviour{
  [SerializeField] private string itemName;

  // Use this for initialization
  void Start() { }

  // Update is called once per frame
  void Update() { }

  private void OnTriggerEnter(Collider other) {
    Debug.Log("New item: " + itemName);
    Managers.Inventory.AddItem(itemName);
    Destroy(this.gameObject);
  }
}