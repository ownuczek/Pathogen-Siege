using UnityEngine;

public class MainMenuTowerAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float scaleFactor = 1.1f;
    [SerializeField] private float animationSpeed = 1f;

    private Vector3 _initialScale;

    private void Start()
    {
        _initialScale = transform.localScale;
    }

    private void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * animationSpeed) * (scaleFactor - 1);
        transform.localScale = _initialScale * scale;
    }
}