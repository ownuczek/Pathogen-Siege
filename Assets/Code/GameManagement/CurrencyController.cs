using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CurrencyController : MonoBehaviour
{
    public static CurrencyController Instance { get; private set; }

    [Header("Currency Settings")]
    [SerializeField] private int defaultCurrency = 100;
    private int _currentCurrency;

    [Header("UI References")]
    [SerializeField] private TMP_Text inGameCurrencyText;
    [SerializeField] private TMP_Text shopCurrencyText;

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
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ResetCurrencyToDefault();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReinitializeUI();
        UpdateCurrencyUI();
    }

    private void ReinitializeUI()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            inGameCurrencyText = canvas.transform.Find("InGameCurrencyText")?.GetComponent<TMP_Text>();
            shopCurrencyText = canvas.transform.Find("ShopCurrencyText")?.GetComponent<TMP_Text>();
        }
    }

    public void ResetCurrencyToDefault()
    {
        _currentCurrency = defaultCurrency;
        UpdateCurrencyUI();

        Debug.Log($"Currency reset. Default currency: {defaultCurrency}");
        ShopManager.Instance?.UpdateCurrencyDisplay();
    }

    public void IncreaseCurrency(int amount)
    {
        if (amount <= 0) return;

        _currentCurrency += amount;
        UpdateCurrencyUI();

        Debug.Log($"Currency increased by {amount}. New total: {_currentCurrency}");
        
        StatisticsManager.Instance?.IncrementCurrencyEarned(amount);

        ShopManager.Instance?.UpdateCurrencyDisplay();
    }


    public bool SpendCurrency(int amount)
    {
        if (_currentCurrency >= amount)
        {
            _currentCurrency -= amount;
            UpdateCurrencyUI();

            Debug.Log($"Currency spent: {amount}. Remaining: {_currentCurrency}");
            ShopManager.Instance?.UpdateCurrencyDisplay();
            return true;
        }

        Debug.LogWarning("Not enough currency.");
        return false;
    }

    public int GetCurrentCurrency()
    {
        return _currentCurrency;
    }

    public void UpdateCurrencyUI()
    {
        if (inGameCurrencyText != null)
        {
            inGameCurrencyText.text = $"Currency: {_currentCurrency}";
        }

        if (shopCurrencyText != null)
        {
            shopCurrencyText.text = $"Currency: {_currentCurrency}";
        }
    }
}
