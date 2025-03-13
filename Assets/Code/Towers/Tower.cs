using UnityEngine;

[System.Serializable]
public class Tower
{
    public string name;
    public int cost;
    public GameObject prefab;
    public Sprite previewImage;

    public Tower(string name, int cost, GameObject prefab, Sprite previewImage)
    {
        this.name = name;
        this.cost = cost;
        this.prefab = prefab;
        this.previewImage = previewImage;
    }
}