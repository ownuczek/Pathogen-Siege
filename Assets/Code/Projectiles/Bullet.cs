using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 10;
    [SerializeField] private float lifetime = 3f;

    private Transform _target;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rb.linearVelocity = direction * bulletSpeed;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetDamage(int damage)
    {
        bulletDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(bulletDamage);

                // Sprawdzamy, czy wróg jest martwy po otrzymaniu obrażeń
                if (enemyHealth.IsDead())
                {
                    // Pobieramy komponent EnemyMovement i dodajemy XP
                    EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
                    if (enemyMovement != null)
                    {
                        int xpReward = enemyMovement.GetReward();
                        PlayerStats.instance.AddXP(xpReward);  // Dodajemy XP
                        Debug.Log($"Dodano {xpReward} XP za zabicie przeciwnika!");
                    }
                }
            }

            Destroy(gameObject);  // Zniszczenie pocisku po trafieniu
        }
    }
}