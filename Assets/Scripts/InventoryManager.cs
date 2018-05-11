using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    
    #region 单例模式
    private static InventoryManager _instance;

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            }
            return _instance;
        }
    }
    #endregion

    private List<Item> itemList;

    #region ToolTip
    private ToolTip toolTip;
    private bool isToolTipShow = false;
    private Vector2 toolTipPositionOffset = new Vector2(10, -10);
    #endregion

    private Canvas canvas;

    #region PickedItem

    private bool isPickedItem;

    public bool IsPickedItem
    {
        get { return isPickedItem; }
        set { isPickedItem = value; }
    }
    private ItemUI pickedItem;
    public ItemUI PickedItem
    {
        get { return pickedItem; }
    }
    #endregion

    void Awake()
    {
        ParseItemJson();
    }
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        toolTip = GameObject.FindObjectOfType<ToolTip>();
        pickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        pickedItem.Hide();
    }

    void Update()
    {
        if (isPickedItem)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);
            pickedItem.SetLocalPosition(position);
        }
        else if (isToolTipShow)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);
            toolTip.SetLocalPosition(position+toolTipPositionOffset);
        }
        if (isPickedItem && Input.GetMouseButtonDown(0) &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false)
        {
            isPickedItem = false;
            pickedItem.Hide();
        }
    }
    void ParseItemJson()
    {
        itemList=new List<Item>();
        TextAsset itemText = Resources.Load<TextAsset>("Items");
        JsonData itemsData=JsonMapper.ToObject(itemText.text);
            
        Item item = null;
        foreach (JsonData itemData in itemsData)
        {
            int id = (int) itemData["id"];
            string itemName = itemData["name"].ToString();
            Item.ItemQuality quality =
                (Item.ItemQuality) Enum.Parse(typeof(Item.ItemQuality), itemData["quality"].ToString());
            string des = itemData["description"].ToString();
            int capacity = (int) itemData["capacity"];
            int buyPrice = (int) itemData["buyPrice"];
            int sellPrice = (int) itemData["sellPrice"];
            string sprite = itemData["sprite"].ToString();
            switch (itemData["type"].ToString())
            {
                case "Consumable":
                    int hp = (int) itemData["hp"];
                    int mp = (int) itemData["mp"];
                    item = new Consumable(id,itemName,Item.ItemType.Consumable,quality,des,capacity,buyPrice,sellPrice,sprite,hp,mp);
                    break;
                case "Equipment":
                    int strength = (int) itemData["strength"];
                    int intellect = (int) itemData["intellect"];
                    int agility = (int) itemData["agility"];
                    int stamina = (int) itemData["stamina"];
                    Equipment.EquipmentType equipmentType =
                        (Equipment.EquipmentType) Enum.Parse(typeof(Equipment.EquipmentType),
                            itemData["equipmentType"].ToString());
                    item = new Equipment(id, itemName, Item.ItemType.Consumable, quality, des, capacity, buyPrice,
                        sellPrice, sprite, strength, intellect, agility, stamina, equipmentType);
                    break;
                case "Weapon":
                    int damage = (int) itemData["damage"];
                    Weapon.WeaponType weaponType = (Weapon.WeaponType) Enum.Parse(typeof(Weapon.WeaponType),
                        itemData["weaponType"].ToString());
                    item=new Weapon(id, itemName, Item.ItemType.Consumable, quality, des, capacity, buyPrice,
                        sellPrice, sprite,damage,weaponType);
                    break;
                case "Material":
                    item = new Material(id, itemName, Item.ItemType.Consumable, quality, des, capacity, buyPrice,
                        sellPrice, sprite);
                    break;
            }
            itemList.Add(item);
            //Debug.Log(item);
        }
        
    }

    public Item GetItemById(int id)
    {
        foreach (Item item in itemList)
        {
            if (item.Id == id)
                return item;
        }
        return null;
    }

    public void ShowToolTip(string content)
    {
        if(isPickedItem)return;
        toolTip.Show(content);
        isToolTipShow = true;

    }
    public void HideToolTip()
    {
        toolTip.Hide();
        isToolTipShow = false;
    }
    
    public void PickupItem(Item item, int amount)
    {
        pickedItem.SetItem(item,amount);
        isPickedItem = true;

        pickedItem.Show();
        toolTip.Hide();
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            Input.mousePosition, null, out position);
        pickedItem.SetLocalPosition(position);
    }
    
    public void RemoveItem(int amount=1)
    {
        pickedItem.ReduceAmount(amount);
        if (pickedItem.Amount <= 0)
        {
            isPickedItem = false;
            pickedItem.Hide();
        }
    }

    public void SaveInventory()
    {
        Knapsack.Instance.SaveInventory();
        Forge.Instance.SaveInventory();
        Chest.Instance.SaveInventory();
        CharacterPanel.Instance.SaveInventory();
        PlayerPrefs.SetInt("coinAmount",GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount);
    }

    public void LoadInventory()
    {
        Knapsack.Instance.LoadInventory();
        Forge.Instance.LoadInventory();
        Chest.Instance.LoadInventory();
        CharacterPanel.Instance.LoadInventory();
        if (PlayerPrefs.HasKey("coinAmount"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount =
                PlayerPrefs.GetInt("coinAmount");
        }
    }
}
