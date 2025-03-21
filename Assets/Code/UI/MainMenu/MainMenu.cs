using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    [Header("UI References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button statisticsButton;
    [SerializeField] private Button achievementsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject statisticsPanel;
    [SerializeField] private GameObject achievementsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Statistics References")]
    [SerializeField] private TMP_Text totalPlaytimeText;
    [SerializeField] private TMP_Text totalEnemiesKilledText;
    [SerializeField] private TMP_Text totalCurrencyEarnedText;
    [SerializeField] private TMP_Text levelsCompletedText;

    [Header("Achievements References")]
    [SerializeField] private TMP_Text achievement1Text;
    [SerializeField] private TMP_Text achievement2Text;
    [SerializeField] private TMP_Text achievement3Text;
    [SerializeField] private TMP_Text achievement4Text;
    [SerializeField] private TMP_Text achievement5Text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playButton.onClick.AddListener(StartNewGame);
        statisticsButton.onClick.AddListener(OpenStatistics);
        achievementsButton.onClick.AddListener(OpenAchievements);
        creditsButton.onClick.AddListener(ShowCredits);
        quitButton.onClick.AddListener(QuitGame);

        statisticsPanel.SetActive(false);
        achievementsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        UpdateStatisticsUI();
        UpdateAchievementsUI();
    }

    private void StartNewGame()
    {
        StatisticsManager.Instance.SaveRuntimeDataToFile();
        SceneManager.LoadScene("Level1");
    }

    private void OpenStatistics()
    {
        statisticsPanel.SetActive(true);
        UpdateStatisticsUI();
    }

    public void CloseStatistics()
    {
        statisticsPanel.SetActive(false);
    }

    private void OpenAchievements()
    {
        achievementsPanel.SetActive(true);
        UpdateAchievementsUI();
    }

    public void CloseAchievements()
    {
        achievementsPanel.SetActive(false);
    }

    private void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    private void QuitGame()
    {
        StatisticsManager.Instance.SaveRuntimeDataToFile();
        Application.Quit();
    }

    public void UpdateStatisticsUI()
    {
        var data = StatisticsManager.Instance.runtimeData;

        System.TimeSpan playtime = System.TimeSpan.FromSeconds(data.totalPlaytime);
        string playtimeFormatted = $"{playtime.Hours:D2}h:{playtime.Minutes:D2}m:{playtime.Seconds:D2}s";

        totalEnemiesKilledText.text = $"Enemies Killed: {data.totalEnemiesKilled}";
        totalCurrencyEarnedText.text = $"Currency Earned: {data.totalCurrencyEarned}";
        levelsCompletedText.text = $"Levels Completed: {data.levelsCompleted}";
        totalPlaytimeText.text = $"Total Playtime: {playtimeFormatted}";
    }

    public void UpdateAchievementsUI()
    {
        var achievements = StatisticsManager.Instance.runtimeData.Achievements;

        achievement1Text.text = $"First Victory - Complete Level 1: {(achievements.ContainsKey("FirstVictory") && achievements["FirstVictory"] ? "Unlocked" : "Locked")}";
        achievement2Text.text = $"Enemy Slayer - Destroy 100 enemies: {(achievements.ContainsKey("EnemySlayer") && achievements["EnemySlayer"] ? "Unlocked" : "Locked")}";
        achievement3Text.text = $"Explorer - Complete 10 levels: {(achievements.ContainsKey("Explorer") && achievements["Explorer"] ? "Unlocked" : "Locked")}";
        achievement4Text.text = $"Master Defender - Survive 20 waves: {(achievements.ContainsKey("MasterDefender") && achievements["MasterDefender"] ? "Unlocked" : "Locked")}";
        achievement5Text.text = $"Time Master - Play for 10 hours: {(achievements.ContainsKey("TimeMaster") && achievements["TimeMaster"] ? "Unlocked" : "Locked")}";
    }
}
