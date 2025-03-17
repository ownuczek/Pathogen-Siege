using UnityEngine;

public class TowerUpgradeManager : MonoBehaviour
{
    public static TowerUpgradeManager instance;
    public GameObject upgradePanel;  // Panel z UI do ulepszania wieży
    private TowerController currentTower;  // Upewnij się, że ta zmienna jest przypisana

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Zachowaj managera przy zmianie sceny
        }
        else
        {
            Destroy(gameObject);  // Uniknij duplikatów managera
        }
    }

    // Otwórz panel ulepszania i przypisz bieżącą wieżę
    public void OpenUpgradePanel(TowerController tower)
    {
        if (tower != null)  // Sprawdzamy, czy tower nie jest null
        {
            currentTower = tower;  // Przypisujemy wieżę
            upgradePanel.SetActive(true);
            UpdateUpgradeText();  // Aktualizuj tekst ulepszania
        }
        else
        {
            Debug.LogWarning("Nie przypisano wieży do ulepszania!");
        }
    }

    // Zamknij panel ulepszania
    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        currentTower = null;  // Resetowanie zmiennej po zamknięciu panelu
    }

    // Ulepsz wieżę, jeśli jest przypisana
    public void UpgradeTower()
    {
        if (currentTower != null)
        {
            currentTower.UpgradeTower();  // Ulepsz wieżę
            UpdateUpgradeText();  // Zaktualizuj tekst po ulepszeniu
        }
        else
        {
            Debug.LogWarning("Nie przypisano wieży do ulepszania!");
        }
    }

    // Zaktualizuj tekst w panelu ulepszania
    private void UpdateUpgradeText()
    {
        if (currentTower != null)
        {
            TowerUpgradeUI upgradeUI = upgradePanel.GetComponent<TowerUpgradeUI>();
            if (upgradeUI != null)
            {
                upgradeUI.UpdateUpgradeText(currentTower);  // Aktualizuj tekst na podstawie aktualnej wieży
            }
        }
        else
        {
            Debug.LogWarning("Nie przypisano wieży do zaktualizowania tekstu!");
        }
    }
}
