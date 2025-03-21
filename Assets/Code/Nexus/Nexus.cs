using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private int damagePerVirus = 10;

    [Header("UI Elements")]
    [SerializeField] private RectTransform greenBar;

    private int _currentHealth;
    private readonly HashSet<EnemyMovement> _attackingViruses = new HashSet<EnemyMovement>();
    private Coroutine _damageCoroutine;
    private float _originalWidth;
    private bool _isDestroyed;

    private void Start()
    {
        ResetNexus();
    }

    public void StartVirusAttack(EnemyMovement virus)
    {
        if (virus == null || _attackingViruses.Contains(virus)) return;

        _attackingViruses.Add(virus);

        if (_damageCoroutine == null)
        {
            _damageCoroutine = StartCoroutine(HandleDamageOverTime());
        }
    }

    public void StopVirusAttack(EnemyMovement virus)
    {
        if (virus == null) return;

        _attackingViruses.Remove(virus);

        // Stop the coroutine if no enemies are attacking
        if (_attackingViruses.Count == 0 && _damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }
    }

    private IEnumerator HandleDamageOverTime()
    {
        while (_attackingViruses.Count > 0 && _currentHealth > 0)
        {
            // Clean up any destroyed or null viruses from the list
            _attackingViruses.RemoveWhere(v => v == null || v.IsDestroyed);

            if (_attackingViruses.Count == 0)
            {
                _damageCoroutine = null;
                yield break;
            }

            _currentHealth -= damagePerVirus * _attackingViruses.Count;
            _currentHealth = Mathf.Max(_currentHealth, 0);
            UpdateHealthBar();

            if (_currentHealth <= 0 && !_isDestroyed)
            {
                DestroyNexus();
                yield break;
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void DestroyNexus()
    {
        if (_isDestroyed) return;

        _isDestroyed = true;
        Debug.Log("Nexus has been destroyed!");

        GameOverManager.Instance?.ShowGameOver();
    }

    public void ResetNexus()
    {
        _currentHealth = maxHealth;
        _attackingViruses.Clear();
        _isDestroyed = false;

        if (_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (greenBar != null)
        {
            if (_originalWidth == 0)
            {
                _originalWidth = greenBar.sizeDelta.x;
            }

            float healthPercentage = (float)_currentHealth / maxHealth;
            greenBar.sizeDelta = new Vector2(_originalWidth * healthPercentage, greenBar.sizeDelta.y);
        }
    }

    public void OnEnemyDestroyed(EnemyMovement virus)
    {
        StopVirusAttack(virus);
    }
}
