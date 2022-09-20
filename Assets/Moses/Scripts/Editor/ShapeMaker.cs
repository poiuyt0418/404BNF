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
        scalar = 1920 / position.width;
        if (center == Vector2.zero)
        {
            Rect shapeRect = shape.FindProperty("shape").rectValue;
            rectSizeX = shapeRect.width / scalar;
            rectSizeY = shapeRect.height / scalar;
            center = shapeRect.center / scalar;
            mouseRect = new Rect(shapeRect.x / scalar + border, (shapeRect.y / scalar) + EditorGUIUtility.singleLineHeight * 4, rectSizeX, rectSizeY);
            rectDiffX = (center.x - rectSizeX / 2) + border;
            rectDiffY = (center.y - rectSizeY / 2) + EditorGUIUtility.singleLineHeight * 4;
        }
        //variableToggle = EditorGUILayout.Toggle("text", variableToggle);
        bool mouseInBound = Event.current.mousePosition.y > EditorGUIUtility.singleLineHeight * 4;
        //Bounds where shape can be made
        EditorGUI.DrawRect(new Rect(border, EditorGUIUtility.singleLineHeight * 4, 1920/scalar, 1080/scalar), new Color(0.9f, 0.9f, 0.9f));
        EditorGUIUtility.AddCursorRect(new Rect(border, EditorGUIUtility.singleLineHeight * 4, position.width, position.height), MouseCursor.Pan); 
        EditorGUILayout.BeginHorizontal();
        center.x = EditorGUILayout.DelayedFloatField("Center X", center.x);
        center.y = EditorGUILayout.DelayedFloatField("Center Y", center.y);
        EditorGUILayout.EndHorizontal(); 
        EditorGUILayout.BeginHorizontal();
        mouseRect.width = EditorGUILayout.DelayedFloatField("Shape Width", mouseRect.width);
        mouseRect.height = EditorGUILayout.DelayedFloatField("Shape Height", mouseRect.height);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);
        if (GUILayout.Button("Submit Shape", GUILayout.Width(100), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
        {
            shape.FindProperty("shape").rectValue = new Rect((mouseRect.x - border) * scalar, (mouseRect.y - EditorGUIUtility.singleLineHeight * 4) * scalar, mouseRect.width * scalar, mouseRect.height * scalar);
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
        mouseRect = new Rect(rectDiffX+border, rectDiffY, rectSizeX, rectSizeY);
        //mouseRect = new Rect(center.x, center.y, rectSizeX, rectSizeY);
        EditorGUI.DrawRect(mouseRect, new Color(0.9f, 0f, 0.9f));
    }
}
