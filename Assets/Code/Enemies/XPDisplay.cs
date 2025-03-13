using UnityEngine;
using TMPro;

public class XPDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI xpText;

    void Update()
    {
        if (PlayerStats.instance != null)
        {
            xpText.text = "XP: " + PlayerStats.instance.GetCurrentXP();
        }
    }
}