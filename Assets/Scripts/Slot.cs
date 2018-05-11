using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{

    public GameObject itemPrefab;
    
    public void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(itemPrefab);
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale=Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }

    public Item.ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }
    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Id;
    }

    public bool IsFull()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            InventoryManager.Instance.HideToolTip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<ItemUI>().Item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(toolTipText);
        }
        else
        {
            InventoryManager.Instance.HideToolTip();
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if (currentItemUI.Item is Equipment || currentItemUI.Item is Weapon)
                {
                    Item currentItem = currentItemUI.Item;
                    currentItemUI.ReduceAmount(1);
                    if (currentItemUI.Amount <= 0)
                    {
                        DestroyImmediate(currentItemUI.gameObject);
                        InventoryManager.Instance.HideToolTip();
                    }
                    CharacterPanel.Instance.PutOn(currentItem);
                }
            }
        }

        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (transform.childCount > 0)
        {
            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
            if (InventoryManager.Instance.IsPickedItem==false)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int amountPicked = (currentItem.Amount + 1) / 2;
                    InventoryManager.Instance.PickupItem(currentItem.Item,amountPicked);
                    int amountRemined = currentItem.Amount - amountPicked;
                    if (amountRemined <= 0)
                    {
                        Destroy(currentItem.gameObject);
                    }
                    else
                    {
                        currentItem.SetAmount(amountRemined);
                    }
                }
                else
                {
                    InventoryManager.Instance.PickupItem(currentItem.Item,currentItem.Amount);
                    Destroy(currentItem.gameObject);
                }
            }
            else
            {
                if (currentItem.Item.Id == InventoryManager.Instance.PickedItem.Item.Id)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (currentItem.Item.Capacity > currentItem.Amount)
                        {
                            currentItem.AddAmount();
                            InventoryManager.Instance.RemoveItem();
                        }
                        else
                        {
                            return;
                        }

                    }
                    else
                    {
                        if (currentItem.Item.Capacity > currentItem.Amount)
                        {
                            int amountRemined = currentItem.Item.Capacity - currentItem.Amount;
                            if (amountRemined >= InventoryManager.Instance.PickedItem.Amount)
                            {
                                currentItem.SetAmount(currentItem.Amount+ InventoryManager.Instance.PickedItem.Amount);
                                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                            }
                            else
                            {
                                currentItem.SetAmount(currentItem.Amount+amountRemined);
                                InventoryManager.Instance.RemoveItem(amountRemined);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Item item = currentItem.Item;
                    int amount = currentItem.Amount;
                    currentItem.SetItem(InventoryManager.Instance.PickedItem.Item,InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.PickedItem.SetItem(item,amount);
                }
            }
        }
        else
        {
            if (InventoryManager.Instance.IsPickedItem)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.RemoveItem();
                }else
                {
                    for (int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++)
                    {
                        this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    }
                    InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                }
            }
            else
            {
                return;
            }
        }
    }
}
