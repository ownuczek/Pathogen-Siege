using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;  // Singleton PlayerStats, aby nie tworzyć nowych instancji

    [Header("XP Settings")]
    [SerializeField] private int currentXP = 0;  // Obecne punkty XP
    [SerializeField] private int xpToLevelUp = 2000;  // Stała wartość XP do awansu (2000)

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI xpText;  // Odwołanie do komponentu TextMeshPro, gdzie wyświetlamy XP

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Utrzymuj obiekt między scenami

            LoadXP();  // Załaduj XP podczas inicjalizacji
            Debug.Log("PlayerStats instance created: " + gameObject.name);
        }
        else
        {
            Destroy(gameObject);  // Zniszcz obiekt, jeśli instancja już istnieje
            Debug.Log("PlayerStats instance already exists. Destroying duplicate: " + gameObject.name);
        }
    }

    private void Start()
    {
        UpdateXPText();  // Zaktualizuj UI wyświetlające punkty XP
    }

    // Funkcja do dodawania XP
    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToLevelUp)
        {
            LevelUp();  // Jeśli przekroczyliśmy wymagane XP, przechodzimy na wyższy poziom
        }
        UpdateXPText();  // Zaktualizuj UI
        SaveXP();  // Zapisz XP
    }

    // Funkcja wywoływana, gdy następuje awans na wyższy poziom
    private void LevelUp()
    {
        currentXP = 0;  // Resetowanie XP po awansie na wyższy poziom
        // xpToLevelUp += 50;  // Zwiększanie wymaganych XP do awansu, ale jeśli chcesz, aby zostało 2000, usuń ten fragment
        Debug.Log("Level Up!");  // Logowanie
    }

    // Funkcja do aktualizacji wyświetlania XP
    public void UpdateXPText()
    {
        if (xpText != null)
        {
            xpText.text = "XP: " + currentXP + "/" + xpToLevelUp;  // Wyświetlamy obecne XP i wymagane do awansu
        }
    }

    // Funkcja do pobrania obecnych punktów XP
    public int GetCurrentXP()
    {
        return currentXP;
    }

    // Zapisanie XP do PlayerPrefs (lub do innego systemu zapisu, np. pliku)
    public void SaveXP()
    {
        PlayerPrefs.SetInt("CurrentXP", currentXP);  // Zapisuje punkty XP do PlayerPrefs
        PlayerPrefs.Save();  // Zapisz zmiany
        Debug.Log("XP saved!");
    }

    // Załadowanie XP z PlayerPrefs
    public void LoadXP()
    {
        if (PlayerPrefs.HasKey("CurrentXP"))
        {
            currentXP = PlayerPrefs.GetInt("CurrentXP");
        }
    }

    // Funkcja do resetowania XP przy kliknięciu restartu
    public void ResetXP()
    {
        currentXP = 0;  // Resetowanie punktów XP
        SaveXP();  // Zapisz nowe wartości XP po resecie
        Debug.Log("XP Reseted!");
    }
}
