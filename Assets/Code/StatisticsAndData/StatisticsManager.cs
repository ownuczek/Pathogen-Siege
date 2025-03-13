using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance;

    [Header("Save File Settings")]
    public string saveFilePath = "saveData.json";

    [Header("Runtime Data")]
    public StatisticsData runtimeData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDataToRuntime();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        runtimeData.totalPlaytime += Time.deltaTime;
        MainMenu.Instance?.UpdateStatisticsUI();
    }

    private void LoadDataToRuntime()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFilePath);

        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);

                runtimeData.totalEnemiesKilled = saveData.totalEnemiesKilled;
                runtimeData.totalCurrencyEarned = saveData.totalCurrencyEarned;
                runtimeData.levelsCompleted = saveData.levelsCompleted;
                runtimeData.totalPlaytime = saveData.totalPlaytime;
                runtimeData.Achievements = new Dictionary<string, bool>(saveData.Achievements);
            }
            catch
            {
                runtimeData.ResetData();
            }
        }
        else
        {
            runtimeData.ResetData();
        }
    }

    public void SaveRuntimeDataToFile()
    {
        if (runtimeData == null)
        {
            Debug.LogError("Runtime data is null. Cannot save.");
            return;
        }

        SaveData saveData = new SaveData
        {
            totalEnemiesKilled = runtimeData.totalEnemiesKilled,
            totalCurrencyEarned = runtimeData.totalCurrencyEarned,
            levelsCompleted = runtimeData.levelsCompleted,
            totalPlaytime = runtimeData.totalPlaytime,
            Achievements = new Dictionary<string, bool>(runtimeData.Achievements)
        };

        string path = Path.Combine(Application.persistentDataPath, saveFilePath);

        try
        {
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, json);
        }
        catch
        {
            Debug.LogError("Failed to save runtime data.");
        }
    }

    public void IncrementEnemiesKilled(int amount)
    {
        runtimeData.totalEnemiesKilled += amount;
        CheckAchievements();
        MainMenu.Instance?.UpdateStatisticsUI();
        Debug.Log($"Total Enemies Killed: {runtimeData.totalEnemiesKilled}");
    }


    public void IncrementCurrencyEarned(int amount)
    {
        if (amount <= 0) return;

        runtimeData.totalCurrencyEarned += amount;
        MainMenu.Instance?.UpdateStatisticsUI();
    }

    public void IncrementLevelsCompleted()
    {
        runtimeData.levelsCompleted++;
        CheckAchievements();
        MainMenu.Instance?.UpdateStatisticsUI();
    }

    private void CheckAchievements()
    {
        if (runtimeData.levelsCompleted >= 1 && !runtimeData.Achievements["FirstVictory"])
        {
            runtimeData.Achievements["FirstVictory"] = true;
        }
        if (runtimeData.totalEnemiesKilled >= 100 && !runtimeData.Achievements["EnemySlayer"])
        {
            runtimeData.Achievements["EnemySlayer"] = true;
        }
        if (runtimeData.levelsCompleted >= 10 && !runtimeData.Achievements["Explorer"])
        {
            runtimeData.Achievements["Explorer"] = true;
        }
        if (runtimeData.totalCurrencyEarned >= 5000 && !runtimeData.Achievements["MasterDefender"])
        {
            runtimeData.Achievements["MasterDefender"] = true;
        }
        if (runtimeData.totalPlaytime >= 36000 && !runtimeData.Achievements["TimeMaster"]) // 10 hours
        {
            runtimeData.Achievements["TimeMaster"] = true;
        }
        MainMenu.Instance?.UpdateAchievementsUI();
    }

    private void OnApplicationQuit()
    {
        SaveRuntimeDataToFile();
    }

    public void ResetData()
    {
        runtimeData.ResetData();
        SaveRuntimeDataToFile();
    }
}
