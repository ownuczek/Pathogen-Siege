using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] private int currentXP = 0;  // Aktualna liczba XP (teraz dostępna w Inspektorze)
    [SerializeField] private int level = 1;      // Aktualny poziom gracza (teraz dostępny w Inspektorze)
    [SerializeField] private int xpToNextLevel = 200;  // Punkty XP wymagane do zdobycia następnego poziomu (teraz dostępne w Inspektorze)

    private void Awake()
    {
        
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

   
    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

   
    public void ResetXP()
    {
        currentXP = 0;
    }

    
    public int GetCurrentXP()
    {
        return currentXP;  
    }

   
    public int GetCurrentLevel()
    {
        return level;  
    }

   
    public int GetXPToNextLevel()
    {
        return xpToNextLevel - currentXP;  
    }

    
    private void LevelUp()
    {
        level++;
        currentXP = currentXP - xpToNextLevel; 
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f); 
    }
}