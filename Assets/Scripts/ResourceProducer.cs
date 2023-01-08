using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirectionEnum;

public class ResourceProducer : ResourceHolder
{
    [SerializeField] GameObject _resourcePrefab;
    [SerializeField] float _initialResourceZ = -0.2f;

    // Generally, does this ResourceHolder have the right to receive any resource
    public override bool IsAllowedToReceive() => false;

    // Generally, does this ResourceHolder have the right to deliver any resource
    public override bool IsAllowedToGive() => true;

    public override bool IsAllowedToReceive(ResourceHolder resourceHolder) => false;

    public override void OnTick()
    {
        if (_heldResource == null)
        {
            Vector3 position = this.transform.position;
            position.z = _initialResourceZ;

            GameObject resourceObject = Instantiate(_resourcePrefab, position, Quaternion.identity);
            _heldResource = resourceObject.GetComponent<Resource>();
        }
    }
}
