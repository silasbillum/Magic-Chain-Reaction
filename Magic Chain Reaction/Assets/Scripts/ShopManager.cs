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
    public UpgradeManager upgradeManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadUpgrades();
        PopulateShop();
    }

    public void PopulateShop()
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

            // Apply upgrade immediately
            ApplyUpgradeEffect(item);

            Debug.Log("Bought: " + item.itemName);
        }
    }

    private void ApplyUpgradeEffect(ShopItem item)
    {
        if (UpgradeManager.Instance == null) return;

        switch (item.itemName)
        {
            case "More Circles":
                UpgradeManager.Instance.moreCirclesLevel = item.bought;
                break;
            case "Extra Shots":
                UpgradeManager.Instance.extraShotsLevel = item.bought;
                break;
            case "Faster Projectiles":
                UpgradeManager.Instance.fasterProjectilesLevel = item.bought;
                break;
            case "Increase Points Earned":
                UpgradeManager.Instance.increasePointsLevel = item.bought;
                break;
            case "More Time":
                UpgradeManager.Instance.moreTimeLevel = item.bought;
                break;
            case "Projectile Count":
                UpgradeManager.Instance.projectileCountLevel = item.bought;
                break;
            case "Faster Circle Spawn":
                UpgradeManager.Instance.fasterCircleSpawnLevel = item.bought;
                break;
            default:
                Debug.LogWarning($"No upgrade logic defined for {item.itemName}");
                break;
        }

        UpgradeManager.Instance.SaveUpgrades();
        UpgradeManager.Instance.ApplyUpgradesToScene();
        FindFirstObjectByType<UpgradeStatsUI>()?.UpdateUI();
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

    public void ResetAllItems()
    {
        foreach (var item in shopItems)
        {
            item.bought = 0;                       // reset in memory
            PlayerPrefs.SetInt(item.itemName + "_bought", 0); // reset in PlayerPrefs
        }
        PlayerPrefs.Save();
    }



}
