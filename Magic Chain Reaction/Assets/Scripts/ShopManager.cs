using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public ScoreManager ScoreManager;
    public Transform shopContainer;
    public GameObject shopItemPrefab;
    public List<ShopItem> shopItems;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadUpgrades();
        PopulateShop();
    }

    private void PopulateShop()
    {
        foreach (ShopItem item in shopItems)
        {
            GameObject obj = Instantiate(shopItemPrefab, shopContainer, false);
            TMP_Text nameText = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            TMP_Text priceText = obj.transform.Find("ItemPrice").GetComponent <TMP_Text>();
            TMP_Text boughtText = obj.transform.Find("BoughtText").GetComponent<TMP_Text>();
            Button buyButton = obj.transform.Find("BuyButton").GetComponent<Button>();

            nameText.text = item.itemName;
            UpdateItemUI(item, priceText, boughtText, buyButton);

            buyButton.onClick.AddListener(() =>
            {
                TryBuyItem(item, priceText, boughtText, buyButton);
            });
        }
    }

    private void TryBuyItem(ShopItem item, TMP_Text priceText, TMP_Text boughtText, Button buyButton)
    {
        int currentPrice = item.basePrice + item.priceIncrease * item.bought;

        if ((item.maxPurchases != -1 && item.bought >= item.maxPurchases))
        {
            Debug.Log("Max purchases reached");
            return;
        }

        if (ScoreManager.currentScore >= currentPrice)
        {
            ScoreManager.currentScore -= currentPrice;
            ScoreManager.UpdateScoreText();

            item.bought++;
            SaveUpgrade(item);

            UpdateItemUI(item, priceText, boughtText, buyButton);

            Debug.Log("Bought: " + item.itemName);
        }
        else
        {
            Debug.Log("Not Enough Points");
        }
    }

    private void UpdateItemUI(ShopItem item, TMP_Text priceText, TMP_Text boughtText, Button buyButton)
    {
        int nextPrice = item.basePrice + item.priceIncrease * item.bought;
        priceText.text = (item.maxPurchases != -1 && item.bought >= item.maxPurchases) ? "-" : nextPrice.ToString();
        boughtText.text = $"{item.bought}/{(item.maxPurchases == -1 ? "∞" : item.maxPurchases.ToString())}";
        buyButton.interactable = (item.maxPurchases == -1 || item.bought <  item.maxPurchases);
    }

    private void SaveUpgrade(ShopItem item)
    {
        PlayerPrefs.SetInt(item.itemName + "_bought", item.bought);
        PlayerPrefs.Save();
    }

    private void LoadUpgrades()
    {
        foreach (ShopItem item in shopItems) 
        {
            item.bought = PlayerPrefs.GetInt(item.itemName + "_bought", 0);
        }
    }

}
