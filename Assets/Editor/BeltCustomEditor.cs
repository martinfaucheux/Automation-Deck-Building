
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Belt))]
public class BeltCustomEditor : Editor
{
    private Belt t;

    private void OnEnable()
    {
        t = target as Belt;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Infer direction"))
        {
            t.InferDirection();
            EditorUtility.SetDirty(t.gameObject);
        }
    }
}