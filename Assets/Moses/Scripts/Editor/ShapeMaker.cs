using UnityEditor;
using UnityEngine;
using System.Collections;

public class ShapeMaker : EditorWindow
{
    int rectSizeX, rectSizeY;
    Stack mouseDot = new Stack();
    [MenuItem("QTE/ShapeEditor")]
    static void InitWindow()
    {
        ShapeMaker window = (ShapeMaker)GetWindowWithRect(typeof(ShapeMaker), new Rect(0, 0, 750, 750));
        window.Show();
    }

    void OnEnable()
    {
    }

    Vector3[] DotToRect(Vector2 center, int size)
    {
        int newSize = size / 2;
        return new Vector3[] {
            new Vector3(center.x - newSize, center.y - newSize, 0),
            new Vector3(center.x + newSize, center.y - newSize, 0),
            new Vector3(center.x + newSize, center.y + newSize, 0),
            new Vector3(center.x - newSize, center.y + newSize, 0)
        };
    }

    void OnGUI()
    {
        //variableToggle = EditorGUILayout.Toggle("text", variableToggle);
        rectSizeX = EditorGUILayout.DelayedIntField("Event Width", rectSizeX);
        rectSizeY = EditorGUILayout.DelayedIntField("Event Height", rectSizeY);
        Vector2 offset = new Vector2(position.width / 2, position.height / 2);
        Handles.color = Color.white;
        Vector3[] rect = new Vector3[] {
            new Vector3(offset.x - rectSizeX/2, offset.y - rectSizeY/2, 0),
            new Vector3(offset.x + rectSizeX/2, offset.y - rectSizeY/2, 0),
            new Vector3(offset.x + rectSizeX/2, offset.y + rectSizeY/2, 0),
            new Vector3(offset.x - rectSizeX/2, offset.y + rectSizeY/2, 0)
        };
        Handles.DrawSolidRectangleWithOutline(rect, Color.white, new Color(0, 0, 0, 1));
        Handles.color = Color.black;
        
        Handles.DrawSolidRectangleWithOutline(DotToRect(offset, 5), Color.black, new Color(0, 0, 0, 1));
        
        Vector2 mousePos = Event.current.mousePosition - offset + new Vector2(rectSizeX/2, rectSizeY/2);
        EditorGUILayout.LabelField("Mouse Position: ", mousePos.ToString());

        if (Event.current.isMouse && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log("clicked");
            mouseDot.Push(DotToRect(new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y), 5));
            Repaint();
        }
        if(mouseDot.Count > 0)
        {
            Handles.DrawSolidRectangleWithOutline((Vector3[])mouseDot.Peek(), Color.black, new Color(0, 0, 0, 1));
        }
    }
}