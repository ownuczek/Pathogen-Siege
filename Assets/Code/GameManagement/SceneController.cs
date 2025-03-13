using UnityEngine;
using UnityEngine.SceneManagement;  // Dodajemy ten namespace, aby używać metod zmiany scen

public class SceneController : MonoBehaviour
{
    // Funkcja do restartu gry
    public void RestartGame()
    {
        // Resetujemy XP przed zmianą sceny
        PlayerStats.instance.ResetXP();
        
        // Możesz również zresetować inne rzeczy, takie jak pozycja gracza, poziom wroga itp.
        
        // Ładowanie bieżącej sceny (restart gry)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Funkcja do przejścia na następny poziom
    public void LoadNextLevel()
    {
        // Zmieniamy scenę na następny poziom
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}