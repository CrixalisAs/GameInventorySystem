using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    protected Slot[] slotlList;

    private float targetAlpha = 1;

    private float smoothing = 4;

    private CanvasGroup canvasGroup;
	// Use this for initialization
	public virtual void Start ()
	{
	    slotlList = GetComponentsInChildren<Slot>();
	    canvasGroup = GetComponent<CanvasGroup>();
	}

    void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < .01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }

    public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemById(id);
        return StoreItem(item);
    }
    public bool StoreItem(Item item)
    {
        if (item == null)
        {
            Debug.Log("要存储的物品id不存在");
            return false;
        }
        if (item.Capacity == 1)
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.Log("没有空的物品槽");
                return false;
            }
            else
            {
                slot.StoreItem(item);
            }
        }
        else
        {
            Slot slot = FindSameIdSlot(item);
            if (slot != null)
            {
                slot.StoreItem(item);
            }
            else
            {
                Slot emptySlot = FindEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.StoreItem(item);
                }
                else
                {
                    Debug.Log("没有空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }

    private Slot FindEmptySlot()
    {
        foreach (Slot slot in slotlList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }

    private Slot FindSameIdSlot(Item item)
    {
        foreach (Slot slot in slotlList)
        {
            if (slot.transform.childCount >= 1 && slot.GetItemId()==item.Id && slot.IsFull()==false)
            {
                return slot;
            }
        }
        return null;
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        targetAlpha = 1;
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        targetAlpha = 0;
    }

    public void DisplaySwitch()
    {
        if (targetAlpha == 0)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    #region save and load

    public void SaveInventory()
    {
        StringBuilder sb=new StringBuilder();
        foreach (Slot slot in slotlList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                sb.Append(itemUI.Item.Id + "," + itemUI.Amount + "-");

            }
            else
            {
                sb.Append("0-");
            }

        }
        PlayerPrefs.SetString(this.gameObject.name,sb.ToString());
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey(this.gameObject.name) == false)
        {
            return;
        }
        string str = PlayerPrefs.GetString(this.gameObject.name);
        string[] itemArray = str.Split('-');
        for (int i = 0; i < itemArray.Length-1; i++)
        {
            string itemStr = itemArray[i];
            if (itemStr != "0")
            {
                string[] temp = itemStr.Split(',');
                int id = int.Parse(temp[0]);
                Item item = InventoryManager.Instance.GetItemById(id);
                int amount = int.Parse(temp[1]);
                for (int j = 0; j < amount; j++)
                {
                    slotlList[i].StoreItem(item);
                }
            }
            

        }
    }

    #endregion
}
