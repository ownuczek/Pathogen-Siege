using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Sprites and Visuals")]
    public Sprite[] directionSprites;
    public Sprite defaultSprite;

    [Header("Firing Configuration")]
    public Transform firingPoint;
    public Vector2[] firingPointOffsets;
    public GameObject bulletPrefab;
    public float smoothRotationSpeed = 5f;
    public float fireRate = 1f;
    public float bulletSpeed = 10f;
    public float bulletDamage = 10f;

    [Header("Range Detection")]
    [SerializeField] private CircleCollider2D rangeCollider;

    private SpriteRenderer _spriteRenderer;
    private Transform _targetEnemy;
    private int _currentSpriteIndex = -1;
    private float _fireCooldown;

    [Header("Tower Leveling")]
    public int towerLevel = 1;
    public int maxLevel = 3;
    public int upgradeCost = 100;
    private float levelUpMultiplier = 1.5f;  // Współczynnik ulepszania (zwiększa obrażenia i inne statystyki)

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = defaultSprite;

        if (rangeCollider == null)
        {
            Debug.LogError("No CircleCollider2D found for range detection!");
        }

        // Resetowanie poziomu wieży przy przejściu do nowego poziomu
        towerLevel = 1;  // Reset poziomu
    }

    private void Update()
    {
        if (_targetEnemy == null)
        {
            FindNextTarget();
        }

        if (_targetEnemy != null)
        {
            RotateAndShootAtTarget();
        }
        else
        {
            ReturnToDefaultPosition();
        }

        if (_fireCooldown > 0)
        {
            _fireCooldown -= Time.deltaTime;
        }
    }

    // Funkcja wywoływana, gdy gracz kliknie przycisk ulepszania wieży
    public void UpgradeTower()
    {
        // Sprawdzenie, czy wieża może być ulepszona
        if (towerLevel < maxLevel && PlayerStats.instance.GetCurrentXP() >= upgradeCost)
        {
            // Pobranie odpowiednich punktów XP i zaktualizowanie statystyk wieży
            PlayerStats.instance.AddXP(-upgradeCost);  // Odejmujemy punkty XP na koszt ulepszenia
            towerLevel++;  // Zwiększenie poziomu wieży
            UpdateTowerStats();  // Zaktualizowanie statystyk wieży

            Debug.Log("Tower upgraded! New level: " + towerLevel);
        }
        else
        {
            Debug.Log("Not enough XP or max level reached.");
        }
    }

    // Funkcja do aktualizacji statystyk wieży po jej ulepszeniu
    private void UpdateTowerStats()
    {
        bulletDamage *= levelUpMultiplier;  // Zwiększamy obrażenia po ulepszeniu
        fireRate *= levelUpMultiplier;  // Zwiększamy szybkość ognia
        bulletSpeed *= levelUpMultiplier;  

        

        Debug.Log("Updated tower stats: Damage - " + bulletDamage + ", Fire rate - " + fireRate + ", Bullet speed - " + bulletSpeed);
    }

    private void RotateAndShootAtTarget()
    {
        Vector2 direction = _targetEnemy.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(GetCurrentAngle(), targetAngle, Time.deltaTime * smoothRotationSpeed);
        SetTowerDirection(angle);
        HandleFiring();
    }

    private void HandleFiring()
    {
        if (_fireCooldown <= 0f)
        {
            GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(_targetEnemy);
                bulletScript.SetDamage((int)bulletDamage);
            }

            _fireCooldown = 1f / fireRate;
        }
    }

    private void FindNextTarget()
    {
        if (rangeCollider != null)
        {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, rangeCollider.radius, LayerMask.GetMask("Enemy"));

            foreach (var enemy in enemiesInRange)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    _targetEnemy = enemy.transform;
                    return;
                }
            }
        }

        _targetEnemy = null;
    }

    private void SetTowerDirection(float angle)
    {
        angle = (angle + 360) % 360;
        int newSpriteIndex = Mathf.FloorToInt((angle + 22.5f) / 45f) % directionSprites.Length;

        if (newSpriteIndex != _currentSpriteIndex)
        {
            _currentSpriteIndex = newSpriteIndex;
            _spriteRenderer.sprite = directionSprites[_currentSpriteIndex];
            UpdateFiringPoint(_currentSpriteIndex);
        }
    }

    private void ReturnToDefaultPosition()
    {
        _spriteRenderer.sprite = defaultSprite;
        _currentSpriteIndex = -1;
        if (firingPoint != null)
        {
            firingPoint.localPosition = Vector2.zero;
        }
    }

    private void UpdateFiringPoint(int spriteIndex)
    {
        if (firingPoint != null && firingPointOffsets.Length == directionSprites.Length)
        {
            firingPoint.localPosition = firingPointOffsets[spriteIndex];
        }
    }

    private float GetCurrentAngle()
    {
        if (_targetEnemy != null)
        {
            Vector2 direction = _targetEnemy.position - transform.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        return 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeCollider != null ? rangeCollider.radius : 5f);
    }
}
