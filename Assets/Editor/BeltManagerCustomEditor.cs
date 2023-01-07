
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BeltManager))]
public class BeltManagerCustomEditor : Editor
{
    private BeltManager t;

    private void OnEnable()
    {
        t = target as BeltManager;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Build Systems"))
        {
            t.BuildSystems();
        }
    }
}