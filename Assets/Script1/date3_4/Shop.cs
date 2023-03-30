using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // Shop - 상점
    //------------------------------------------------------------------------------
    private static Dictionary<int, Item>         _itemSet         = null;
    private static Dictionary<int, PurchaseInfo> _purchaseInfoSet = null;

    //private static EItemType _currentTab = EItemType.Unknown;
    //사용안함경고

    public static void ChangeTab(EItemType itemType)
    {
        // 탭에 따라 다른 상점 노출 되도록 수정
    }


    public static void TryAddCoinWithAds(int increaseCoin, Action doneCallback)
    {
        AdsManager.ShowAds();
    }

    /// <summary>
    /// 아이템 장착을 시도합니다.
    /// </summary>
    public static bool TryEquip(int itemId)
    {
        // 아이템 있는지 확인
        if (!_itemSet.TryGetValue(itemId, out var item))
        {
            Debug.LogError($"[Shop.TryEquip] 해당 아이템을 찾을 수 없습니다. id: {itemId}");
            return false;
        }

        // 아이템 구매했는지 확인
        if (!_purchaseInfoSet.TryGetValue(itemId, out var info)
            || null == info
            || info.NeedCost != info.PurchasedCost)
        {
            Debug.LogError($"[Shop.TryEquip] 구매하지 않은 아이템 입니다. id: {itemId}");
            return false;
        }

        // 아이템 장착
        // Player.EquipItem(item);

        return true;
    }


    /// <summary>
    /// 아이템 구매를 시도합니다.
    /// </summary>

    public static void TryBuy(int itemId, Action<bool> doneCallback)
    {
        // 아이템 있는지 확인
        if (!_itemSet.TryGetValue(itemId, out var item))
        {
            Debug.LogError($"[Shop.TryBuy] 해당 아이템을 찾을 수 없습니다. id: {itemId}");
            doneCallback?.Invoke(false);
            return;
        }

        // 아이템 이미 구매했는지 확인
        if (_purchaseInfoSet.TryGetValue(itemId, out var info)
            && null != info
            && info.NeedCost == info.PurchasedCost)
        {
            Debug.LogError($"[Shop.TryBuy] 이미 구매한 아이템 입니다. id: {itemId}");
            doneCallback?.Invoke(false);
            return;
        }

        // 타입에 따라 구매 시도
        switch (item.PriceType)
        {
        case EPriceType.Coin:
            // 코인 체크
            if (UserData.Coin < item.Cost)
            {
                Debug.LogError($"[Shop.TryBuy] 돈이 부족합니다. id: {itemId}");
                doneCallback?.Invoke(false);
                return;
            }

            IncreaseCostCount(item, item.Cost);
            doneCallback?.Invoke(true);
            return;

        case EPriceType.Ads:
            AdsManager.ShowAds();
            
            break;
        }
    }

    /// <summary>
    /// 코스트 지불량 증가
    /// </summary>
    private static void IncreaseCostCount(Item item, int amount)
    {
        if (null == item)
            return;

        if (!_purchaseInfoSet.TryGetValue(item.Id, out var info))
        {
            info = new PurchaseInfo();

            // 정보 초기화
            info.Init(item);

            // 풀에 추가
            _purchaseInfoSet.Add(item.Id, info);
        }

        // 지불량 증가
        info.IncreaseCost(amount);

        // 즉시 착용 해야하는지 확인
        if (info.NeedCost == info.PurchasedCost
            && EItemType.Skin == item.ItemType)
        {
            TryEquip(item.Id);
        }
    }        
}
