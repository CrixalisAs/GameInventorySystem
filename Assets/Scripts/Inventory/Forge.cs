using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class Forge : Inventory
{
    #region 单例模式

    private static Forge _instance;

    public static Forge Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("ForgePanel").GetComponent<Forge>();
            }
            return _instance;
        }
    }
    #endregion
    private List<Formula> formulaList;

    void Awake()
    {
        ParseFormulaJson();
    }
    void ParseFormulaJson()
    {
        formulaList = new List<Formula>();
        TextAsset itemText = Resources.Load<TextAsset>("Formulas");
        JsonData itemsData = JsonMapper.ToObject(itemText.text);
        foreach (JsonData itemData in itemsData)
        {
            int item1ID = (int)itemData["Item1ID"];
            int item1Amount = (int)itemData["Item1Amount"];
            int item2ID = (int)itemData["Item2ID"];
            int item2Amount = (int)itemData["Item2Amount"];
            int resID = (int)itemData["ResID"];
            Formula item=new Formula(item1ID,item1Amount,item2ID,item2Amount,resID);
            formulaList.Add(item);
        }
    }

    public void ForgeItem()
    {
        List<int> haveMeterialIDList=new List<int>();
        foreach (var slot in slotlList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI currentItemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                for (int i = 0; i < currentItemUI.Amount; i++)
                {
                    haveMeterialIDList.Add(currentItemUI.Item.Id);
                }
            }
        }
        Formula matchedFormula = null;
        foreach (Formula formula in formulaList)
        {
            bool isMatch = formula.Match(haveMeterialIDList);
            if (isMatch)
            {
                matchedFormula = formula;
                break;
            }
        }
        if (matchedFormula != null)
        {
            Knapsack.Instance.StoreItem(matchedFormula.ResID);
            foreach (int id in matchedFormula.NeedIdList)
            {
                foreach (var slot in slotlList)
                {
                    if (slot.transform.childCount > 0)
                    {
                        ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                        if (itemUI.Item.Id == id)
                        {
                            if (itemUI.Amount > 0)
                            {
                                itemUI.ReduceAmount();
                                if (itemUI.Amount <= 0)
                                {
                                    DestroyImmediate(itemUI.gameObject);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
