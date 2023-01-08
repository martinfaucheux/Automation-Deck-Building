using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HexCollider))]
public class Resource : MonoBehaviour
{
    [SerializeField] HexCollider _hexCollider;

    void Start()
    {
        ResourceHolder resourceHolder = BeltManager.instance.GetBeltAtPos(_hexCollider.position);
        if (resourceHolder != null)
            resourceHolder.SetHeldResource(this);

    }

    public void Move(Vector2Int position)
    {
        Vector3 targetPosition = HexGrid.instance.GetWorldPos(position);
        targetPosition.z = transform.position.z;

        _hexCollider.position = position;
        LeanTween.move(
            gameObject,
            targetPosition,
            BeltManager.instance.moveDuration
        );
    }
}
