using UnityEngine;
using UnityEngine.EventSystems;

public class BuildPoint : MonoBehaviour
{
    private GameObject _tower;

    public bool IsOccupied()
    {
        return _tower != null;
    }

    private void Start()
    {
        if (!IsOccupied())
        {
            BuildManager.Instance?.RefreshHighlights(); // Odświeżamy highlighty po starcie
        }
    }

    private void OnMouseDown()
    {
        if (IsOccupied())
        {
            Debug.LogWarning("BuildPoint: This point is already occupied.");
            return;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("BuildPoint: Ignored click due to UI interaction.");
            return;
        }

        Debug.Log("BuildPoint: Opening shop for tower selection...");
        BuildManager.Instance.SetPendingBuildPoint(this);
        ShopManager.Instance.OpenShop();
    }

    public void BuildTower(Tower selectedTower)
    {
        if (selectedTower == null)
        {
            Debug.LogWarning("BuildPoint: No tower selected for building.");
            return;
        }

        if (IsOccupied())
        {
            Debug.LogWarning("BuildPoint: This point is already occupied.");
            return;
        }

        if (!CurrencyController.Instance.SpendCurrency(selectedTower.cost))
        {
            Debug.LogWarning("BuildPoint: Not enough currency.");
            return;
        }

        _tower = Instantiate(selectedTower.prefab, transform.position, Quaternion.identity);
        Debug.Log($"BuildPoint: Tower {selectedTower.name} built successfully.");

        BuildManager.Instance.OnTowerBuilt(this); // 🔹 Usunięcie podświetlenia po budowie
        BuildManager.Instance.ClearPendingBuildPoint(); // 🔹 Reset zapisanej pozycji
        ShopManager.Instance.UpdateCurrencyDisplay();
    }
}