using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Main;

    public int CurrentLevel { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject levelCompletedPanel;
    [SerializeField] private GameObject shopButton;
    [SerializeField] private LevelDisplay levelDisplay;
    [SerializeField] private GameObject shopManagerPrefab;

    [Header("Path and Start Point")]
    public Transform StartPoint { get; private set; }
    public Transform[] path;

    [Header("Level Transition Settings")]
    [SerializeField] private float levelTransitionDelay = 3f;

    private void Awake()
    {
        if (Main == null)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            DisableLevelManager();
            return;
        }

        if (scene.name.StartsWith("Level"))
        {
            StartCoroutine(InitializeLevelAfterCanvasLoad());
        }
    }

    private void DisableLevelManager()
    {
        this.enabled = false;
        
        if (levelDisplay != null)
        {
            Destroy(levelDisplay.gameObject);
            levelDisplay = null;
        }
    }

    private IEnumerator InitializeLevelAfterCanvasLoad()
    {
        yield return new WaitUntil(() => UnityEngine.Object.FindFirstObjectByType<Canvas>() != null);
        InitializeLevel(SceneManager.GetActiveScene());
        InitializeUIReferences();
        InitializeSpawner();
        InitializeShop();
    }

    private void InitializeLevel(Scene scene)
    {
        CurrencyController.Instance?.ResetCurrencyToDefault();

        SetupStartPoint();
        SetupPath();
        SetupLevelDisplay(scene);

        ShopManager.Instance?.UpdateCurrencyDisplay();
    }



    private void SetupStartPoint()
    {
        StartPoint = GameObject.Find("StartPoint")?.transform ?? new GameObject("DefaultStartPoint").transform;
        Debug.Log(StartPoint != null ? "StartPoint initialized." : "StartPoint not found in the scene.");
    }

    private void SetupPath()
    {
        GameObject pathObject = GameObject.Find("Path");
        path = pathObject?.GetComponentsInChildren<Transform>().Skip(1).ToArray() ?? System.Array.Empty<Transform>();

        if (path.Length > 0)
        {
            Debug.Log($"Path initialized with {path.Length} waypoints.");
        }
        else
        {
            Debug.LogError("Path object not found or empty.");
        }
    }

    private void SetupLevelDisplay(Scene scene)
    {
        if (int.TryParse(scene.name.Replace("Level", ""), out int levelNumber))
        {
            CurrentLevel = levelNumber;
            Debug.Log($"Current level set to {CurrentLevel}.");
            levelDisplay?.UpdateLevelDisplay(CurrentLevel);
        }
        else
        {
            Debug.LogError("Failed to parse level number from scene name.");
        }
    }

    private void InitializeUIReferences()
    {
        levelCompletedPanel = FindInCanvas("LevelCompletedPanel");
        shopButton = FindInCanvas("ShopButton");

        levelDisplay = UnityEngine.Object.FindAnyObjectByType<LevelDisplay>();
        if (levelDisplay != null)
        {
            levelDisplay.UpdateLevelDisplay(CurrentLevel);
            Debug.Log("LevelDisplay updated with current level.");
        }
        else
        {
            Debug.LogError("LevelDisplay not found in the scene.");
        }
    }

    private GameObject FindInCanvas(string objectName)
    {
        Canvas[] canvases = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var canvas in canvases)
        {
            Transform target = canvas.transform.Find(objectName);
            if (target != null)
            {
                return target.gameObject;
            }
        }
        return null;
    }

    private void InitializeSpawner()
    {
        EnemySpawner enemySpawner = UnityEngine.Object.FindFirstObjectByType<EnemySpawner>();
        enemySpawner?.InitializeSpawner();
    }

    private void InitializeShop()
    {
        if (ShopManager.Instance == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null && !canvas.activeInHierarchy)
            {
                canvas.SetActive(true);
                Debug.Log("Canvas activated to initialize ShopManager.");
            }

            if (ShopManager.Instance == null)
            {
                Debug.LogError("ShopManager is not found in the scene.");
            }
        }
        else
        {
            Debug.Log("ShopManager already initialized.");
        }
    }


    public void RestartLevel()
    {
        if (CurrencyController.Instance != null)
        {
            CurrencyController.Instance.ResetCurrencyToDefault();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelWithDelay());
    }

    private IEnumerator LoadNextLevelWithDelay()
    {
        yield return new WaitForSeconds(levelTransitionDelay);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ShowLevelCompleted()
    {
        if (levelCompletedPanel == null)
        {
            Debug.LogError("LevelCompletedPanel is not assigned in the inspector.");
            return;
        }

        levelCompletedPanel.SetActive(true);
        shopButton?.SetActive(false);
        Time.timeScale = 0f;
        StatisticsManager.Instance?.IncrementLevelsCompleted();

        StartCoroutine(LoadNextLevelAfterDelay());
    }
    
    private IEnumerator LoadNextLevelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(levelTransitionDelay);
        Time.timeScale = 1f;
        LoadNextLevel();
    }

    public void HideLevelCompleted()
    {
        levelCompletedPanel?.SetActive(false);
        shopButton?.SetActive(true);
        Time.timeScale = 1f;
    }
}
