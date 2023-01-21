public class ResourceConsumer : ResourceHolder
{
    public override bool IsAllowedToGive() => false;

    public override bool IsAllowedToReceive() => true;

    public override bool IsAllowedToReceiveFrom(ResourceHolder resourceHolder) => true;

    public override void OnTick()
    {
        if (_heldResource != null)
        {
            Destroy(_heldResource.gameObject);
            _heldResource = null;
        }
    }
}
