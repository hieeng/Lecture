using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseInfo
{
    //------------------------------------------------------------------------------
    // PurchaseInfo - 구매한 아이템 혹은 구매중인 아이템 정보
    //------------------------------------------------------------------------------
    public Item Item            { get; private set; }
    public int  PurchasedCost   { get; private set; }
    public int  NeedCost        { get; private set; }

    public void Init(Item item)
    {
        Item          = item;
        PurchasedCost = 0;
        NeedCost      = item.Cost;
    }

    public void IncreaseCost(int amount)
    {
        PurchasedCost += amount;
    }
}
