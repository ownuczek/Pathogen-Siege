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

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = defaultSprite;

        if (rangeCollider == null)
        {
            Debug.LogError("No CircleCollider2D found for range detection!");
        }
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