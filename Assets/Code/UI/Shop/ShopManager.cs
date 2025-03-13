using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private GameObject towerButtonPrefab;
    [SerializeField] private Transform shopContent;

    [Header("Tower Data")]
    [SerializeField] private List<Tower> towers;

    private bool _isShopOpen;
    private readonly List<ShopItem> _shopItems = new List<ShopItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }

        PopulateShop();
        UpdateCurrencyDisplay();
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            _isShopOpen = true;
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            _isShopOpen = false;
        }
    }

    public bool IsShopOpen()
    {
        return _isShopOpen;
    }

    public void PopulateShop()
    {
        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject);
        }

        _shopItems.Clear();

        for (int i = 0; i < towers.Count; i++)
        {
            GameObject button = Instantiate(towerButtonPrefab, shopContent);
            ShopItem shopItem = button.GetComponent<ShopItem>();

            if (shopItem != null)
            {
                shopItem.Initialize(towers[i]); 
                shopItem.buyButton.onClick.AddListener(() =>
                {
                    BuildPoint selectedPoint = BuildManager.Instance.GetPendingBuildPoint();

                    if (selectedPoint != null)
                    {
                        selectedPoint.BuildTower(towers[i]);
                        BuildManager.Instance.ClearPendingBuildPoint();
                    }

                    CloseShop();
                    UpdateCurrencyDisplay();
                });
                _shopItems.Add(shopItem);
            }
        }

        UpdateCurrencyDisplay();
    }

    public void UpdateCurrencyDisplay()
    {
        if (currencyUI != null && CurrencyController.Instance != null)
        {
            currencyUI.text = $"Currency: {CurrencyController.Instance.GetCurrentCurrency()}";
        }

        foreach (ShopItem item in _shopItems)
        {
            if (item != null)
            {
                int towerCost = item.GetTowerCost();
                bool canAfford = CurrencyController.Instance.GetCurrentCurrency() >= towerCost;
                item.buyButton.interactable = canAfford;
                item.UpdateUI();
            }
        }
    }
}
