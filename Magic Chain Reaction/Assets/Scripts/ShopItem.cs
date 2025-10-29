using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public int basePrice;
    public int priceIncrease;
    public int maxPurchases;
    public int bought = 0;
}
