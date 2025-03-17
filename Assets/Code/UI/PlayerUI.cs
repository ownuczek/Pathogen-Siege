using UnityEngine;
using TMPro;  // Używamy TextMeshPro

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text xpText;                // TMP_Text do wyświetlania XP
    [SerializeField] private TMP_Text levelText;             // TMP_Text do wyświetlania aktualnego poziomu

    void Update()
    {
        // Wyświetlanie aktualnych punktów XP oraz liczby XP do kolejnego poziomu
        int currentXP = PlayerStats.instance.GetCurrentXP();
        int xpToNextLevel = PlayerStats.instance.GetXPToNextLevel();

        // Zmieniamy sposób wyświetlania na krótszą formę: "XP: 150 (+40)"
        xpText.text = "XP: " + currentXP + " (+ " + xpToNextLevel + ")";

        // Wyświetlanie numeru poziomu gracza w formacie: "Lvl 1"
        levelText.text = "Lvl " + PlayerStats.instance.GetCurrentLevel();
    }
}