using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Powiadomienie EnemySpawner o zniszczeniu wroga
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null && enemyMovement.enemySpawner != null)
        {
            enemyMovement.enemySpawner.EnemyDestroyed(enemyMovement);
        }

        // Usunięcie obiektu wroga
        Destroy(gameObject);
    }
}