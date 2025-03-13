using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private string nextSceneName = "Level2";  // Nazwa kolejnej sceny do załadowania

    private void Update()
    {
        // Naciśnięcie przycisku "P" powoduje przejście do kolejnego poziomu
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadNextLevel();
        }
    }

    // Funkcja ładowania nowego poziomu
    public void LoadNextLevel()
    {
        // Zapisz obecny stan XP przed przejściem
        int playerXP = PlayerStats.instance.GetCurrentXP();
        Debug.Log("Przechodzimy do następnej sceny, aktualne XP: " + playerXP);  // Logowanie

        // Zmieniamy scenę
        SceneManager.LoadScene(nextSceneName);

        // Po załadowaniu nowej sceny, przypisujemy XP z poprzedniego poziomu
        PlayerStats.instance.AddXP(playerXP);
    }
}