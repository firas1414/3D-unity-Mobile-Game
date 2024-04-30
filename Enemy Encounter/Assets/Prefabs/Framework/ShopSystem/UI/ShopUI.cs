using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] ShopSystem shopSystem;
    [SerializeField] ShopItemUI shopItemUIPrefab;
    [SerializeField] RectTransform shopList;
    [SerializeField] CreditComponent creditComp;
    [SerializeField] UIManager uiManager;

    [SerializeField] TextMeshProUGUI creditText;

    [SerializeField] Button BackBtn;
    [SerializeField] Button BuyBtn;

    List<ShopItemUI> shopItems = new List<ShopItemUI>();

    ShopItemUI selectedItem;

    private void Start()
    {
        InitShopItems();
        BackBtn.onClick.AddListener(uiManager.SwithToGameplayUI);
        BuyBtn.onClick.AddListener(TryPuchaseItem);
        creditComp.onCreditChanged += UpdateCredit;
        UpdateCredit(creditComp.Credit);
    }

    private void TryPuchaseItem()
    {
        if (!selectedItem || !shopSystem.TryPurchase(selectedItem.GetItem(), creditComp))
            return;

        RemoveItem(selectedItem);
    }

    private void RemoveItem(ShopItemUI itemToRemove)
    {
        shopItems.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
    }

    private void UpdateCredit(int newCredit)
    {
        creditText.SetText(newCredit.ToString());
        RefreshItems();
    }

    private void RefreshItems()
    {
        foreach(ShopItemUI shopItemUI in shopItems)
        {
            shopItemUI.Refresh(creditComp.Credit);
        }
    }

    private void InitShopItems() // RETRIEVE THE SHOP ITEMS FROM THE shopSystem Class(Scriptable Object), AND PUT THEM IN THE shopItems LIST, THEN ADD THEIR UI's to THE ShopUI
    {
        ShopItem[] shopItems = shopSystem.GetShopItems(); 
        foreach(ShopItem item in shopItems)
        {
            AddShopItem(item);
        }
    }

    private void AddShopItem(ShopItem item) // ADD A ShopItem's UI to the ShopUI Prefab
    {
        ShopItemUI newItemUI = Instantiate(shopItemUIPrefab, shopList); // CREATE A ShopItemUI FOR THE ITEM IN THE GAMEPLAY
        newItemUI.Init(item, creditComp.Credit);
        newItemUI.onItemSelected += ItemSelected;
        shopItems.Add(newItemUI); // PUT THE ShopItemUI's IN THE shopItems LIST
    }

    private void ItemSelected(ShopItemUI Item)
    {
        selectedItem = Item;
    }
}
