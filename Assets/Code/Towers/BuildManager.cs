using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("Build Settings")]
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private List<Tower> towerPrefabs;

    private BuildPoint _pendingBuildPoint;
    private readonly Dictionary<BuildPoint, GameObject> _highlightedPoints = new Dictionary<BuildPoint, GameObject>();
    private List<BuildPoint> _buildPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeBuildPoints();
    }

    private void InitializeBuildPoints()
    {
        _buildPoints = new List<BuildPoint>(Object.FindObjectsByType<BuildPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None));

        foreach (var point in _buildPoints)
        {
            if (!point.IsOccupied())
            {
                AddHighlight(point);
            }
        }
    }

    private void AddHighlight(BuildPoint point)
    {
        if (highlightPrefab != null && !_highlightedPoints.ContainsKey(point))
        {
            GameObject highlight = Instantiate(highlightPrefab, point.transform.position, Quaternion.identity, point.transform);
            _highlightedPoints[point] = highlight;
        }
    }

    public void RemoveHighlight(BuildPoint point)
    {
        if (_highlightedPoints.ContainsKey(point))
        {
            Destroy(_highlightedPoints[point]);
            _highlightedPoints.Remove(point);
        }
    }

    public void RefreshHighlights()
    {
        foreach (var point in _buildPoints)
        {
            if (!point.IsOccupied() && !_highlightedPoints.ContainsKey(point))
            {
                AddHighlight(point);
            }
            else if (point.IsOccupied() && _highlightedPoints.ContainsKey(point))
            {
                RemoveHighlight(point);
            }
        }
    }

    public void OnTowerBuilt(BuildPoint point)
    {
        RemoveHighlight(point);
        RefreshHighlights();
    }

    // 🔹 **Brakujące metody do obsługi wyboru miejsca budowy**
    public void SetPendingBuildPoint(BuildPoint point)
    {
        _pendingBuildPoint = point;
    }

    public BuildPoint GetPendingBuildPoint()
    {
        return _pendingBuildPoint;
    }

    public void ClearPendingBuildPoint()
    {
        _pendingBuildPoint = null;
    }
}