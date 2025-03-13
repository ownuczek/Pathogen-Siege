using UnityEngine;
using TMPro;

public class XPDisplay : MonoBehaviour
{
    public TextMeshProUGUI xpText;

    void Update()
    {
        if (PlayerStats.instance != null)
        {
            xpText.text = "XP: " + PlayerStats.instance.experiencePoints;
        }
    }
}