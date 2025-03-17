using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class TowerUpgradeUI : MonoBehaviour
{
    public Text upgradeButtonText;  // Przypisz ten komponent w Inspectorze, jeśli używasz zwykłego Text
    // public TMP_Text upgradeButtonText; // Użyj tego, jeśli używasz TextMeshPro
    public Button upgradeButton;    // Przypisz ten komponent w Inspectorze, jeśli masz przycisk ulepszania

    // Zaktualizuj tekst przycisku ulepszania na podstawie stanu wieży
    public void UpdateUpgradeText(TowerController tower)
    {
        if (tower != null)
        {
            if (tower.towerLevel < tower.maxLevel)
            {
                upgradeButtonText.text = "Upgrade (" + tower.upgradeCost + " XP)";
            }
            else
            {
                upgradeButtonText.text = "Max Level Reached";
            }
        }
    }

    
    public void OnUpgradeButtonClicked()
    {
        if (upgradeButton != null)
        {
            TowerUpgradeManager.instance.UpgradeTower();
        }
    }
}