
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexCollider))]
public class HexColliderCustomEditor : Editor
{
    private HexCollider t;

    private void OnEnable()
    {
        t = target as HexCollider;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Sync position"))
        {
            t.SyncPosition();
            EditorUtility.SetDirty(t.gameObject);
        }
    }
}