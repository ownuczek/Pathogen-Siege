using UnityEngine;

public class ColorPulseEffect : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public Color baseColor = Color.white;
    public float pulseIntensity = 0.5f;

    private Renderer _objectRenderer;
    private Material _material;

    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        _material = _objectRenderer.material;
    }

    void Update()
    {
        float intensity = (Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f) * pulseIntensity;
        _material.color = baseColor * (1 + intensity);
    }
}