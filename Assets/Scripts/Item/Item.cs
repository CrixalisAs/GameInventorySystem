using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品基类
/// </summary>
public class Item  {

	public int Id { get; set; }
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public ItemQuality Quality { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
    public int BuyPrice { get; set; }
    public int SellPrice { get; set; }
    public string Sprite { get; set; }

    public Item()
    {
        Id = -1;
    }
    public Item(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice,
        int sellPrice,string sprite)
    {
        this.Id = id;
        this.Name = name;
        this.Type = type;
        this.Quality = quality;
        this.Description = des;
        this.Capacity = capacity;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
        this.Sprite = sprite;
    }

    /// <summary>
    /// 物品类型
    /// </summary>
    public enum ItemType
    {
        Consumable,
        Equipment,
        Weapon,
        Material
    }

    /// <summary>
    /// 品质类型
    /// </summary>
    public enum ItemQuality
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Artifact
    }

    public override string ToString()
    {
        return string.Format("Id: {0}, Name: {1}, Type: {2}, Quality: {3}, Description: {4}, Capacity: {5}, BuyPrice: {6}, SellPrice: {7}", Id, Name, Type, Quality, Description, Capacity, BuyPrice, SellPrice);
    }

    public virtual string GetToolTipText()
    {
        return String.Format("<color=red>{0}</color>",Name);
    }
}
