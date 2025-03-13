using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatisticsData", menuName = "Game/StatisticsData")]
public class StatisticsData : ScriptableObject
{
    public int totalEnemiesKilled;
    public int totalCurrencyEarned;
    public int levelsCompleted;
    public float totalPlaytime;
    public Dictionary<string, bool> Achievements = new Dictionary<string, bool>
    {
        { "FirstVictory", false },
        { "EnemySlayer", false },
        { "Explorer", false },
        { "MasterDefender", false },
        { "TimeMaster", false }
    };

    public void ResetData()
    {
        totalEnemiesKilled = 0;
        totalCurrencyEarned = 0;
        levelsCompleted = 0;
        totalPlaytime = 0;
        Achievements = new Dictionary<string, bool>
        {
            { "FirstVictory", false },
            { "EnemySlayer", false },
            { "Explorer", false },
            { "MasterDefender", false },
            { "TimeMaster", false }
        };
    }
}