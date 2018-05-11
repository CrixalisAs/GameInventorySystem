using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    #region Data
    public Item Item { get; private set; }
    public int Amount { get; private set; }
    #endregion
    
    #region UI Component
    private Image itemImage;
    private Text amountText;

    private Image ItemImage
    {
        get
        {
            if (itemImage == null)
            {
                itemImage = GetComponent<Image>();
            }
            return itemImage;
        }
    }

    private Text AmountText
    {
        get
        {
            if (amountText == null)
            {
                amountText = GetComponentInChildren<Text>();
            }
            return amountText;
        }
    }
    #endregion

    private Vector3 targetScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 animationScale = new Vector3(1.2f, 1.2f, 1.2f);
    private float smoothing = 4f;

    void Update()
    {
        if (transform.localScale != targetScale)
        {
            float scale = Mathf.Lerp(transform.localScale.x, targetScale.x, Time.deltaTime*smoothing);
            transform.localScale=new Vector3(scale,scale,scale);
            if (Mathf.Abs(transform.localScale.x - targetScale.x) < 0.02f)
            {
                transform.localScale = targetScale;
            }
        }
    }
    public void SetItem(Item item, int amount = 1)
    {
        transform.localScale = animationScale;
        this.Item = item;
        this.Amount = amount;
        //update UI
        ItemImage.sprite = Resources.Load<Sprite>(item.Sprite);
        AmountText.text = Amount.ToString();
    }
    public void AddAmount(int amount=1)
    {
        transform.localScale = animationScale;
        this.Amount += amount;
        if (Item.Capacity > 1)
        {
            AmountText.text = Amount.ToString();
        }
        else
        {
            AmountText.text = "";
        }
        
    }

    public void ReduceAmount(int amount = 1)
    {
        transform.localScale = animationScale;
        this.Amount -= amount;
        if (Item.Capacity > 1)
        {
            AmountText.text = Amount.ToString();
        }
        else
        {
            AmountText.text = "";
        }
    }

    public void SetAmount(int amount)
    {
        transform.localScale = animationScale;
        this.Amount = amount;
        if (Item.Capacity > 1)
        {
            AmountText.text = Amount.ToString();
        }
        else
        {
            AmountText.text = "";
        }
    }

    public void Exchange(ItemUI itemUI)
    {
        Item itemTemp = itemUI.Item;
        int amountTemp = itemUI.Amount;
        itemUI.SetItem(this.Item,this.Amount);
        this.SetItem(itemTemp,amountTemp);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
}
