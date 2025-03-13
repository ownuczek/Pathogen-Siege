using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("UI References")]
    public Image towerImage;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI towerNameText;
    public Button buyButton;

    private Tower _towerData;

    public void Initialize(Tower data)
    {
        if (data == null)
        {
            Debug.LogError("ShopItem: Tower data is null.");
            return;
        }

        _towerData = data;

        if (towerImage != null)
        {
            towerImage.sprite = data.previewImage;
        }

        if (costText != null)
        {
            costText.text = $"{data.cost}";
        }

        if (towerNameText != null)
        {
            towerNameText.text = data.name;
        }

        UpdateUI();

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    public void UpdateUI()
    {
        if (_towerData == null || towerImage == null || buyButton == null)
        {
            Debug.LogWarning("ShopItem: UI references or tower data are not properly assigned.");
            return;
        }

        if (CurrencyController.Instance != null && CurrencyController.Instance.GetCurrentCurrency() >= _towerData.cost)
        {
            towerImage.color = Color.white;
            buyButton.interactable = true;
        }
        else
        {
            towerImage.color = new Color(1f, 1f, 1f, 0.5f);
            buyButton.interactable = false;
        }
    }

    private void OnBuyButtonClicked()
    {
        if (_towerData == null)
        {
            Debug.LogWarning("ShopItem: No tower data available for purchase.");
            return;
        }

        BuildPoint selectedPoint = BuildManager.Instance.GetPendingBuildPoint();

        if (selectedPoint != null)
        {
            selectedPoint.BuildTower(_towerData);
            BuildManager.Instance.ClearPendingBuildPoint();
        }

        ShopManager.Instance?.CloseShop();
    }

    public int GetTowerCost()
    {
        return _towerData != null ? _towerData.cost : 0;
    }
}
