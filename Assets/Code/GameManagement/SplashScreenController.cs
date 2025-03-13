using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayTime = 3f;
    public string nextSceneName = "MainMenu";
    public Image logoImage;

    private void Start()
    {
        if (logoImage == null) return;
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        yield return Fade(0, 1, fadeDuration);
        yield return new WaitForSeconds(displayTime);
        yield return Fade(1, 0, fadeDuration);
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = logoImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            logoImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        logoImage.color = color;
    }
}