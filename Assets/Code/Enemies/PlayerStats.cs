using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int experiencePoints = 0;

    private void Awake()
    {
        // Upewniamy się, że instancja jest jedyna (Singleton)
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddXP(int amount)
    {
        experiencePoints += amount;
        Debug.Log("Dodano XP: " + amount + " | Łącznie: " + experiencePoints);
    }
}