
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGridLayout))]
public class HexGridLayoutCustomEditor : Editor
{
    private HexGridLayout t;

    private void OnEnable()
    {
        t = target as HexGridLayout;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Build Grid"))
        {
            t.LayoutGrid();
            EditorUtility.SetDirty(t.gameObject);
        }
    }
}