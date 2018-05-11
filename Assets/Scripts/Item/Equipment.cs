using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item {
    /// <summary>
    /// 力量
    /// </summary>
    public int Strength { get; set; }
    /// <summary>
    /// 智力
    /// </summary>
    public int Intellect { get; set; }
    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility { get; set; }
    /// <summary>
    /// 体力
    /// </summary>
    public int Stamina { get; set; }
    public EquipmentType EquipType { get; set; }

    public Equipment(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice,
        int sellPrice,string sprite, int strength, int intellect, int agility, int stamina, EquipmentType equiptype) : base(id, name, type, quality, des,
        capacity, buyPrice, sellPrice,sprite)
    {
        this.Strength = strength;
        this.Intellect = intellect;
        this.Agility = agility;
        this.Stamina = stamina;
        this.EquipType = equiptype;
    }

    public enum EquipmentType
    {
        None,
        Head,
        Neck,
        Chest,
        Ring,
        Leg,
        Bracer,
        Boots,
        Shoulder,
        Belt,
        OffHand
    }
}
