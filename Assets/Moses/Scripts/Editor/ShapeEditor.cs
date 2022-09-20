using System;
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
        if(GUILayout.Button("Create Shape"))
        {
            ShapeMaker window = (ShapeMaker)EditorWindow.GetWindow(typeof(ShapeMaker),false, "Shape Maker");
            window.ShapeToChange(serializedObject);
            //ShapeMaker window = EditorWindow.GetWindow<ShapeMaker>(typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow"));
        }
    }
}
