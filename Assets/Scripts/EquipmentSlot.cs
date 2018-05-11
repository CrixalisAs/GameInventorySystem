using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot {

    public Equipment.EquipmentType EquipType;
    public Weapon.WeaponType WpType;
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem==false && transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                Item itemTemp = currentItemUI.Item;
                DestroyImmediate(currentItemUI.gameObject);
                transform.parent.parent.SendMessage("PutOff", itemTemp);
                InventoryManager.Instance.HideToolTip();
            }
        }

        if (eventData.button != PointerEventData.InputButton.Left) return;

        bool isUpdateProperty = false;
        if (InventoryManager.Instance.IsPickedItem == true)
        {
            ItemUI pickedItem = InventoryManager.Instance.PickedItem;
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if (IsRightItem(pickedItem.Item))
                {
                    pickedItem.Exchange(currentItemUI);
                    isUpdateProperty = true;
                }
            }
            else
            {
                if (IsRightItem(pickedItem.Item))
                {
                    this.StoreItem(pickedItem.Item);
                    InventoryManager.Instance.RemoveItem(1);
                    isUpdateProperty = true;
                }
            }
        }
        else
        {
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                InventoryManager.Instance.PickupItem(currentItemUI.Item,currentItemUI.Amount);
                DestroyImmediate(currentItemUI.gameObject);
                isUpdateProperty = true;
            }
        }
        if (isUpdateProperty)
        {
            transform.parent.parent.SendMessage("UpdatePropertyText");
        }
    }

    public bool IsRightItem(Item item)
    {
        if ((item is Equipment && ((Equipment)item).EquipType == this.EquipType) ||
            (item is Weapon && ((Weapon)item).WpType == this.WpType))
        {
            return true;
        }
        return false;
    }
}
