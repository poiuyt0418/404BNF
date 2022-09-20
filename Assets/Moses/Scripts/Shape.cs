using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Shape")]
public class Shape : ScriptableObject
{
    [Tooltip("Enclosing Rectangle of the Shape")]
    public Rect shape;

    public Vector2 Center()
    {
        return shape.center;
    }
    public Vector2 StartCorner()
    {
        return shape.min;
    }
    public Vector2 EndCorner()
    {
        return shape.max;
    }
}