using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject blockerPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private bool _isGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Time.timeScale = 1f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            InitializeUIReferences();
        }
        else
        {
            ResetState();
        }
    }

    private void InitializeUIReferences()
    {
        if (gameOverPanel == null || blockerPanel == null || restartButton == null || mainMenuButton == null)
        {
            GameObject canvas = GameObject.Find("Canvas");

            if (canvas != null)
            {
                gameOverPanel = canvas.transform.Find("GameOverPanel")?.gameObject;
                blockerPanel = canvas.transform.Find("BlockerPanel")?.gameObject;
                restartButton = gameOverPanel?.transform.Find("RestartButton")?.GetComponent<Button>();
                mainMenuButton = gameOverPanel?.transform.Find("MainMenuButton")?.GetComponent<Button>();
            }
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (blockerPanel != null) blockerPanel.SetActive(false);

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void ShowGameOver()
    {
        if (_isGameOver) return;

        _isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (blockerPanel != null) blockerPanel.SetActive(true);

        EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();
        spawner?.ResetSpawnerForGameOver();
    }

    public void RestartGame()
    {
        ResetState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        ResetState();
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetState()
    {
        _isGameOver = false;
        Time.timeScale = 1f;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (blockerPanel != null) blockerPanel.SetActive(false);
    }
}
