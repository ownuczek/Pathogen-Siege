using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] private int currentXP = 0;  // Aktualna liczba XP (teraz dostępna w Inspektorze)
    [SerializeField] private int level = 1;      // Aktualny poziom gracza (teraz dostępny w Inspektorze)
    [SerializeField] private int xpToNextLevel = 200;  // Punkty XP wymagane do zdobycia następnego poziomu (teraz dostępne w Inspektorze)

    private void Awake()
    {
        // Zapewnienie, że istnieje tylko jedna instancja PlayerStats
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Metoda do dodawania XP
    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    // Metoda do resetowania XP
    public void ResetXP()
    {
        currentXP = 0;
    }

    // Metoda do uzyskania aktualnych XP
    public int GetCurrentXP()
    {
        return currentXP;  // Zwracamy bieżące punkty XP
    }

    // Metoda do uzyskania aktualnego poziomu gracza
    public int GetCurrentLevel()
    {
        return level;  // Zwracamy aktualny poziom gracza
    }

    // Metoda do uzyskania XP potrzebnych do osiągnięcia kolejnego poziomu
    public int GetXPToNextLevel()
    {
        return xpToNextLevel - currentXP;  // Zwracamy różnicę między XP do kolejnego poziomu
    }

    // Funkcja do przejścia na wyższy poziom
    private void LevelUp()
    {
        level++;
        currentXP = currentXP - xpToNextLevel;  // Resetujemy XP do następnego poziomu
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);  // Zwiększamy ilość XP do kolejnego poziomu
    }
}