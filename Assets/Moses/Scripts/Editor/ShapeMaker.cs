using UnityEditor;
using UnityEngine;
using System.Collections;

public class ShapeMaker : EditorWindow
{
    float rectSizeX, rectSizeY, rectDiffX, rectDiffY;
    Stack mouseDot = new Stack();
    bool pressed = false;
    Vector2 center;
    Rect mouseRect;
    SerializedObject shape;
    float scalar = 1;
    float border = 10;
    //[MenuItem("QTE/ShapeEditor")]
    static void InitWindow()
    {
        //ShapeMaker window = (ShapeMaker)GetWindowWithRect(typeof(ShapeMaker), new Rect(0, 0, 750, 750));
        //ShapeMaker window = (ShapeMaker)GetWindow(typeof(ShapeMaker),true,"Shape Maker");
        //window.Show();
        
    }

    public void ShapeToChange(SerializedObject a)
    {
        shape = a;
    }

    void Awake()
    {

    }

    void OnGUI()
    {
        scalar = (position.width - border) / 1920;
        //Bounds where shape should be made
        Rect boundRect = new Rect(border, EditorGUIUtility.singleLineHeight * 4, 1920 * scalar, 1080 * scalar);
        if (center == Vector2.zero)
        {
            Rect shapeRect = shape.FindProperty("shape").rectValue;
            rectSizeX = shapeRect.width * boundRect.width;
            rectSizeY = shapeRect.height * boundRect.height;
            center = new Vector2(shapeRect.center.x * boundRect.width + border, shapeRect.center.y * boundRect.height + EditorGUIUtility.singleLineHeight * 4);
            //mouseRect = new Rect(shapeRect.x / boundRect.width + border, (shapeRect.y / boundRect.height) + EditorGUIUtility.singleLineHeight * 4, rectSizeX, rectSizeY);
            rectDiffX = center.x - rectSizeX / 2;
            rectDiffY = center.y - rectSizeY / 2;
        }
        //variableToggle = EditorGUILayout.Toggle("text", variableToggle);
        bool mouseInBound = Event.current.mousePosition.y > EditorGUIUtility.singleLineHeight * 4;
        EditorGUI.DrawRect(boundRect, new Color(0.9f, 0.9f, 0.9f));
        EditorGUIUtility.AddCursorRect(new Rect(border, EditorGUIUtility.singleLineHeight * 4, position.width, position.height), MouseCursor.Pan); 
        EditorGUILayout.BeginHorizontal();
        rectDiffX = EditorGUILayout.DelayedFloatField("Center X", rectDiffX);
        rectDiffY = EditorGUILayout.DelayedFloatField("Center Y", rectDiffY);
        EditorGUILayout.EndHorizontal(); 
        EditorGUILayout.BeginHorizontal();
        rectSizeX = EditorGUILayout.DelayedFloatField("Shape Width", rectSizeX);
        rectSizeY = EditorGUILayout.DelayedFloatField("Shape Height", rectSizeY);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);
        if (GUILayout.Button("Submit Shape", GUILayout.Width(100), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
        {
            shape.FindProperty("shape").rectValue = new Rect((mouseRect.x - border) / boundRect.width, (mouseRect.y - EditorGUIUtility.singleLineHeight * 4) / boundRect.height, mouseRect.width / boundRect.width, mouseRect.height / boundRect.height);
            shape.ApplyModifiedProperties();
        }
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown && Event.current.button == 0 && !pressed && mouseInBound)
        {
            pressed = true;
            center = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);
            
        } else if(pressed)
        {
            if(mouseInBound)
            {
                if (Event.current.isMouse && Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    pressed = false;
                }
                rectSizeX = Mathf.Abs(Event.current.mousePosition.x - center.x) * 2;
                rectSizeY = Mathf.Abs(Event.current.mousePosition.y - center.y) * 2;
                rectDiffX = center.x - Mathf.Abs(Event.current.mousePosition.x - center.x);
                rectDiffY = center.y - Mathf.Abs(Event.current.mousePosition.y - center.y);
                
            }
            //Handles.DrawSolidRectangleWithOutline((Vector3[])mouseDot.Peek(), Color.black, new Color(0, 0, 0, 1));
            Repaint();
        }
        mouseRect = new Rect(rectDiffX, rectDiffY, rectSizeX, rectSizeY);
        //mouseRect = new Rect(center.x, center.y, rectSizeX, rectSizeY);
        EditorGUI.DrawRect(mouseRect, new Color(0.9f, 0f, 0.9f));
    }
}
