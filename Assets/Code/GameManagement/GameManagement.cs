using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManagement : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject levelCompletedPanel;
    [SerializeField] private GameObject blockerPanel;
    [SerializeField] private GameObject shopButton;
    [SerializeField] private TextMeshProUGUI levelCompletedText;

    [Header("Settings")]
    [SerializeField] private float levelTransitionDelay = 3f;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeUIReferences();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            InitializeUIReferences();
        }
    }

    private void InitializeUIReferences()
    {
        if (levelCompletedPanel == null || blockerPanel == null || shopButton == null || levelCompletedText == null)
        {
            GameObject canvas = GameObject.Find("Canvas");

            if (canvas != null)
            {
                levelCompletedPanel ??= FindChildObject(canvas, "LevelCompletedPanel");
                blockerPanel ??= FindChildObject(canvas, "BlockerPanel");
                shopButton ??= FindChildObject(canvas, "ShopButton");
                levelCompletedText ??= levelCompletedPanel?.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        blockerPanel?.SetActive(false);
        levelCompletedPanel?.SetActive(false);
    }

    private GameObject FindChildObject(GameObject parent, string childName)
    {
        Transform child = parent.transform.Find(childName);
        return child != null ? child.gameObject : null;
    }

    public void ShowLevelCompleted()
    {
        if (blockerPanel == null || levelCompletedPanel == null)
        {
            Debug.LogWarning("UI references are missing. Unable to show level completed panel.");
            return;
        }

        blockerPanel.SetActive(true);
        levelCompletedPanel.SetActive(true);
        shopButton?.SetActive(false);

        if (levelCompletedText != null)
        {
            levelCompletedText.text = "Level Completed!";
        }

        StartCoroutine(LoadNextLevelAfterDelay(levelTransitionDelay));
    }

    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
