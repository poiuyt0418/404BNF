using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Shape")]
public class Shape : ScriptableObject
{
    [Tooltip("x,y of each corner")]
    public Vector2[] ShapeCorners;
    public Vector2 offsetXY = Vector2.zero;
    public Vector2 Offset()
    {
        return offsetXY+new Vector2(Screen.width/2,Screen.height/2);
    }
    public Vector2 Min()
    {
        return new Vector2(ShapeCorners[3].x, ShapeCorners[3].y);
    }
    public Vector2 Max()
    {
        return new Vector2(ShapeCorners[1].x, ShapeCorners[1].y);
    }
}