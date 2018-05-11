using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region basic property
    private int basicStrength = 10;
    private int basicIntellect = 10;
    private int basicAgility = 10;
    private int basicStamina = 10;
    private int basicDamage = 10;

    public int BasicStrength
    {
        get { return basicStrength; }
    }

    public int BasicIntellect
    {
        get { return basicIntellect; }
    }

    public int BasicAgility
    {
        get { return basicAgility; }
    }

    public int BasicStamina
    {
        get { return basicStamina; }
    }

    public int BasicDamage
    {
        get { return basicDamage; }
    }
    #endregion

    private int coinAmount = 100;
    private Text coinText;

    public int CoinAmount
    {
        get { return coinAmount; }
        set
        {
            coinAmount = value;
            coinText.text = coinAmount.ToString();
        }
    }

    void Start()
    {
        coinText = GameObject.Find("Coin").GetComponentInChildren<Text>();
        coinText.text = coinAmount.ToString();
    }
    void Update()
    {
        coinText.text = coinAmount.ToString();
        if (Input.GetKeyDown(KeyCode.G))
        {
            int id = Random.Range(1, 4);
            Knapsack.Instance.StoreItem(id);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Knapsack.Instance.DisplaySwitch();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Chest.Instance.DisplaySwitch();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            CharacterPanel.Instance.DisplaySwitch();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Vendor.Instance.DisplaySwitch();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Forge.Instance.DisplaySwitch();
        }
    }

    public bool ConsumeCoin(int amount)
    {
        if (coinAmount >= amount)
        {
            coinAmount -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void EarnCoin(int amount)
    {
        coinAmount += amount;
    }
}
