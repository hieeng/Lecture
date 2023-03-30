using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    //------------------------------------------------------------------------------
    // Item - 아이템 데이터 클래스
    //------------------------------------------------------------------------------

    public int          Id              { get; private set; }
    public string       Name            { get; private set; }
    public string       ThumbnailPath   { get; private set; } 
    public EItemType    ItemType        { get; private set; } 
    public EItemGrade   ItemGrade       { get; private set; } 
    public EPriceType   PriceType       { get; private set; } 
    public int          Cost            { get; private set; }
    public int          Level           { get; private set; }

	public Item() { }
}	
    //------------------------------------------------------------------------------
    // enums
    //------------------------------------------------------------------------------
    public enum EItemGrade
    {
        Unknown,

        Normal,
        Epic,
    }

    public enum EItemType
    {
        Unknown,

        Weapon,
        Skin,
    }

    public enum EPriceType
    {
        Unknown,

        Coin,
        Ads,
    }