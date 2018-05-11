using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula
{

    public int Item1ID { get; set; }
    public int Item1Amount { get; set; }
    public int Item2ID { get; set; }
    public int Item2Amount { get; set; }
    public int ResID { get; set; }
    private List<int> needIdList = new List<int>();

    public List<int> NeedIdList
    {
        get { return needIdList; }
    }
    public Formula(int item1ID, int item1Amount, int item2ID, int item2Amount, int resID)
    {
        this.Item1ID = item1ID;
        this.Item1Amount = item1Amount;
        this.Item2ID = item2ID;
        this.Item2Amount = item2Amount;
        this.ResID = resID;
        for (int i = 0; i < Item1Amount; i++)
        {
            needIdList.Add(Item1ID);
        }
        for (int i = 0; i < Item2Amount; i++)
        {
            needIdList.Add(Item2ID);
        }
    }

    public bool Match(List<int> idList)
    {
        
        
        List<int> tempIdList=new List<int>(idList);
        foreach (var id in needIdList)
        {
            bool isSuccess = tempIdList.Remove(id);
            if (isSuccess == false)
            {
                return false;
            }
        }
        return true;
    }

}
