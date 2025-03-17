using UnityEngine;

public class UpgradePanelSpawner : MonoBehaviour
{
    public GameObject upgradePanelPrefab;

    void Start()
    {
        if (TowerUpgradeManager.instance != null && TowerUpgradeManager.instance.upgradePanel == null)
        {
            GameObject panel = Instantiate(upgradePanelPrefab);
            TowerUpgradeManager.instance.upgradePanel = panel;
        }
    }
}