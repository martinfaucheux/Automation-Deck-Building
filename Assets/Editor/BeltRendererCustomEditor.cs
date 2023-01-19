
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BeltRenderer))]
public class BeltRendererCustomEditor : Editor
{
    private BeltRenderer t;

    private void OnEnable()
    {
        t = target as BeltRenderer;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Render"))
        {
            t.RenderBelts();
        }
    }
}