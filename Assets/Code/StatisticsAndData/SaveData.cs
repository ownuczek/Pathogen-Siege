using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
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
}