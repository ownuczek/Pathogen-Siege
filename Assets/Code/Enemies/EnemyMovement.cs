using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public EnemySpawner enemySpawner;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int reward = 10;

    private bool _isDestroyed;
    private bool _isRewarded;

    private Transform[] _pathPoints;
    private int _pathIndex;
    private Transform _target;

    public bool IsDestroyed => _isDestroyed;

    private void Start()
    {
        if (LevelManager.Main == null || LevelManager.Main.path == null || LevelManager.Main.path.Length == 0)
        {
            Debug.LogError($"{name}: Path points are not initialized in LevelManager.");
            return;
        }

        _pathPoints = LevelManager.Main.path;
        transform.position = LevelManager.Main.StartPoint.position;
        _target = _pathPoints[0];
    }

    private void Update()
    {
        if (_isDestroyed || _target == null) return;

        if (Vector2.Distance(transform.position, _target.position) <= 0.1f)
        {
            _pathIndex++;

            if (_pathIndex >= _pathPoints.Length)
            {
                ReachedEndPoint();
                return;
            }
            else
            {
                _target = _pathPoints[_pathIndex];
            }
        }

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void ReachedEndPoint()
    {
        rb.linearVelocity = Vector2.zero;

        Nexus nexus = FindObjectOfType<Nexus>();
        if (nexus != null)
        {
            nexus.StartVirusAttack(this);
        }
        else
        {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        if (_isDestroyed) return;

        _isDestroyed = true;

        if (enemySpawner != null && !_isRewarded)
        {
            enemySpawner.EnemyDestroyed(this);
            MarkAsRewarded();
        }

        Nexus nexus = FindObjectOfType<Nexus>();
        nexus?.OnEnemyDestroyed(this);

        Destroy(gameObject);
    }

    public int GetReward() => reward;

    public bool IsRewarded => _isRewarded;

    public void MarkAsRewarded()
    {
        if (_isRewarded) return;

        _isRewarded = true;
        Debug.Log($"{name}: Marked as rewarded.");
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        Debug.Log($"{name}: Speed set to {moveSpeed}");
    }
}
