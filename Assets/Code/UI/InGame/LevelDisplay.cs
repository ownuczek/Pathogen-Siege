using UnityEngine;
using TMPro;
using System.Collections;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 3f;

    private Color _initialColor;

    private void Awake()
    {
        if (levelText == null)
        {
            GameObject currentLevelTextObj = GameObject.Find("CurrentLevelText");

            if (currentLevelTextObj != null)
            {
                levelText = currentLevelTextObj.GetComponent<TextMeshProUGUI>();
            }

            if (levelText == null)
            {
                return;
            }
        }

        _initialColor = levelText.color;
        levelText.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, 0);
        levelText.gameObject.SetActive(false);
    }
    
    public void UpdateLevelDisplay(int level)
    {
        if (levelText == null)
        {
            return;
        }

        levelText.text = $"LEVEL {level}";
        StopAllCoroutines();
        StartCoroutine(ShowLevelText());
    }

    private IEnumerator ShowLevelText()
    {
        levelText.gameObject.SetActive(true);
        yield return FadeIn();
        yield return new WaitForSeconds(displayDuration);
        yield return FadeOut();
        levelText.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            levelText.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            levelText.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, alpha);
            yield return null;
        }
    }
}