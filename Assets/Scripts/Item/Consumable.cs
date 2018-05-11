using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消耗品类
/// </summary>
public class Consumable : Item {

	public int HP { get; set; }
    public int MP { get; set; }

    public Consumable(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice,
        int sellPrice,string sprite, int hp, int mp) : base(id, name, type, quality, des, capacity, buyPrice, sellPrice,sprite)
    {
        this.HP = hp;
        this.MP = mp;
    }

    public override string ToString()
    {
        return string.Format("{0}, HP: {1}, MP: {2}", base.ToString(), HP, MP);
    }
}
