using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Shape))]
[CanEditMultipleObjects]
public class ShapeEditor : Editor
{
    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Shape shape = (Shape)target;
        if(GUILayout.Button("Create Shape"))
        {
            ShapeMaker window = (ShapeMaker)EditorWindow.GetWindow(typeof(ShapeMaker), false, "a");
            window.Show();
        }
    }
}
