
using UnityEditor;
using UnityEngine;

public class ResourceHolderCustomEditor : Editor
{
    private ResourceHolder t;

    private void OnEnable()
    {
        t = target as ResourceHolder;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Infer direction"))
        {
            t.InferDirection();
            EditorUtility.SetDirty(t.gameObject);
            PrefabUtility.RecordPrefabInstancePropertyModifications(t);
        }

        if (GUILayout.Button("Infer all directions"))
        {
            foreach (ResourceHolder resourceHolder in FindObjectsOfType<ResourceHolder>())
            {
                EditorUtility.SetDirty(resourceHolder.gameObject);
                PrefabUtility.RecordPrefabInstancePropertyModifications(resourceHolder);
                resourceHolder.InferDirection();
            }
        }


    }
}

[CustomEditor(typeof(Belt))]
public class BeltCustomEditor : ResourceHolderCustomEditor { }


[CustomEditor(typeof(ResourceProducer))]
public class ResourceProducerCustomEditor : ResourceHolderCustomEditor { }

[CustomEditor(typeof(ResourceConsumer))]
public class ResourceConsumerCustomEditor : ResourceHolderCustomEditor { }