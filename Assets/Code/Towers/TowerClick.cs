using UnityEngine;

public class TowerClick : MonoBehaviour
{
    private TowerController towerController;

    private void Start()
    {
        towerController = GetComponent<TowerController>();
    }

    private void OnMouseDown()
    {
        if (towerController != null)
        {
            TowerUpgradeManager.instance.OpenUpgradePanel(towerController);
        }
    }
}