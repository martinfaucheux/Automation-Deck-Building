using DirectionEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[Serializable]
public class BeltPrefabSet
{
    public GameObject straight;
    public GameObject smallTurn;
    public GameObject wideTurn;
}


public class BeltRenderer : ResourceHolderRenderer
{
    [SerializeField] Belt _belt;
    [SerializeField] BeltPrefabSet _prefabSet;
    [SerializeField] float _height = 0f;
    [SerializeField] float _heightStep = 0.1f;

    private void Start()
    {
        Render();
    }

    public override void Render()
    {
        ResetSprites();

        float height = _height;
        List<ResourceHolder> neighborHolders = _belt.GetNeighbors(feederOnly: true);
        if (neighborHolders.Any())
        {
            foreach (ResourceHolder neighborHolder in neighborHolders)
            {
                DrawBeltLane(neighborHolder.direction, _belt.direction, height);
                height += _heightStep;
            }
        }
        else
        {
            DrawBeltLane(_belt.direction, _belt.direction, height);
        }

    }

    private GameObject DrawBeltLane(Direction inputDirection, Direction outputDirection, float height)
    {
        int angleDiff = inputDirection.Opposite().GetAngleTo(outputDirection);
        GameObject prefab = AngleDiffToPrefab(angleDiff);

        GameObject beltGameObject = GameObject.Instantiate(prefab, transform);
        Vector3 position = transform.position;
        position.z = height;
        beltGameObject.transform.position = position;

        bool inverseSprite = angleDiff > 3 * 60;
        if (inverseSprite)
        {
            Vector3 scale = beltGameObject.transform.localScale;
            scale.y *= -1;
            beltGameObject.transform.localScale = scale;
        }

        Quaternion rotation = Quaternion.Euler(0f, 0f, inputDirection.Opposite().ToAngleDegree());
        beltGameObject.transform.rotation = rotation;
        return beltGameObject;
    }

    private void ResetSprites()
    {
        for (int childCount = 0; childCount < transform.childCount; childCount++)
            Destroy(transform.GetChild(0).gameObject);
    }

    private GameObject AngleDiffToPrefab(int angleDiff)
    {
        GameObject prefab = null;
        switch (angleDiff / 60)
        {
            case 0:
                Debug.LogError("Cannot Draw a U-Turn Belt");
                return null;
            case 1:
                prefab = _prefabSet.smallTurn;
                break;
            case 2:
                prefab = _prefabSet.wideTurn;
                break;
            case 3:
                prefab = _prefabSet.straight;
                break;
            case 4:
                prefab = _prefabSet.wideTurn;
                break;
            case 5:
                prefab = _prefabSet.smallTurn;
                break;
            default:
                prefab = _prefabSet.straight; ;
                break;
        }
        return prefab;
    }

}
